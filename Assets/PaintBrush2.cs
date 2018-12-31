using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
 
public class PaintBrush2 : MonoBehaviour
{
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public Texture2D brushTexture;
    Vector2 stored;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
    }
 
    void Update()
    {
 
        Debug.DrawRay(transform.position, transform.forward * 12, Color.magenta);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 12))
        {
            Collider hitCollider = hit.collider;
            if (hitCollider == null)
            {
                return;
            }
            
            if (!paintTextures.ContainsKey(hitCollider)) // if there is already paint on the material, add to that
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();
                paintTextures.Add(hitCollider, GetWhiteRT());
                rend.material.SetTexture("_PaintMap", paintTextures[hitCollider]);
                /*
                var mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                var total = mesh.bounds.extents.x + mesh.bounds.extents.y + mesh.bounds.extents.z;
                var finalValue = Mathf.RoundToInt(total) * 150;
                
                double result = Mathf.Sqrt(finalValue);
                
                Debug.Log(CompareSquare(finalValue));
                resolution = CompareSquare(finalValue);*/
            }
                
               
            stored = hit.lightmapCoord;
            Vector2 pixelUV = hit.lightmapCoord;
            pixelUV.y *= resolution;
            pixelUV.x *= resolution;
            DrawTexture(paintTextures[hitCollider], pixelUV.x, pixelUV.y);
                
            if (stored != hit.lightmapCoord) // stop drawing on the same point
            {

            }
        }
    }
 
    void DrawTexture(RenderTexture rt, float posX, float posY)
    {
 
        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        
        rt.filterMode = FilterMode.Point;
        
        
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
 
        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
 
 
    }
 
    RenderTexture GetWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }
 
    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }
    
    int roundUp(int numToRound, int multiple)
    {
        if (multiple == 0)
            return numToRound;

        int remainder = numToRound % multiple;
        if (remainder == 0)
            return numToRound;

        return numToRound + multiple - remainder;
    }

    int CompareSquare(int numToRound)
    {
        var min = 4;
        var minPrevious = 2;
        var max = 512;

        while (min < max)
        {
            minPrevious = min;
            min *= 2;
            
            if (numToRound > max)
                return max;
            
            if (numToRound > minPrevious && numToRound <= min)
                return min;
            
        }
                
        return max;
        
        
    }
}