using UnityEngine;

public class PaintRaycast : MonoBehaviour
{
    // Attach this script to a camera and it will paint black pixels in 3D
    // on whatever the user clicks. Make sure the mesh you want to paint
    // on has a mesh collider attached.

    private string paintTex = "_PaintTex";
    Camera cam;
    public Color color;
    int _MyFloatID;
 
    void Awake() {
        _MyFloatID = Shader.PropertyToID("_PaintTex");
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {

        // Only if we hit something, do we continue
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 100);
        
        if (!Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            return;
        }

        // Just in case, also make sure the collider also has a renderer
        // material and texture. Also we should ignore primitive colliders.
        Renderer rend = hit.transform.GetComponent<Renderer>();
        

        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || /*rend.sharedMaterial == null || */rend.sharedMaterial.mainTexture == null || meshCollider == null)
        {
            Debug.Log(rend);
            Debug.Log(meshCollider);
            Debug.Log(rend.sharedMaterial);
            Debug.Log(rend.sharedMaterial.mainTexture);
            //return;
        }
        
        // Now draw a pixel where we hit the object
        //Texture2D tex = rend.material.mainTexture as Texture2D;
        
        Texture2D tex = rend.material.GetTexture(_MyFloatID) as Texture2D;
        if (tex == null)
        {
            tex = new Texture2D(512,512, TextureFormat.RGBA32, false, false);
            tex.filterMode = FilterMode.Point;
            rend.material.SetTexture(_MyFloatID, tex);
        }
        
        Vector2 pixelUV = hit.lightmapCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        tex.SetPixel(Mathf.FloorToInt(pixelUV.x), Mathf.FloorToInt(pixelUV.y), color);

        tex.Apply();
    }
}