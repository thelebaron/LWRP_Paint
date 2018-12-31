using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GetBounds : MonoBehaviour
{
    public Bounds bounds;
    public Vector3 boundsExtents;
    public Vector3 minBounds;
    public Vector3 maxBounds;

    public int finalValue;
    private Renderer render;

    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        bounds = mesh.bounds;
        boundsExtents = bounds.extents;
        minBounds = bounds.min;
        maxBounds = bounds.max;

        var total = mesh.bounds.extents.x + mesh.bounds.extents.y + mesh.bounds.extents.z;
        finalValue = Mathf.RoundToInt(total);
    }
}
