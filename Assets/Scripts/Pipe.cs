using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float curveRadius, pipeRadius;
    public int curveSegCount;
    public int pipeSegCount;

    private Mesh mesh;
    private Vector3[] verts;
    private int[] triangles;

    public float ringDistance;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Pipe";
        SetVertices();
        SetTriangles();
        mesh.RecalculateNormals();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetPointOnTorus(float u, float v)
    {
        Vector3 p;
        float r = (curveRadius + pipeRadius * Mathf.Cos(v));

        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = pipeRadius * Mathf.Sin(v);

        return p;
    }

    /*
    private void OnDrawGizmos()
    {
        float uStep = (2f * Mathf.PI) / curveSegCount;
        float vStep = (2f * Mathf.PI) / pipeSegCount;

        for (int u = 0; u < curveSegCount; u++)
        {
            for (int v = 0; v < pipeSegCount; v++)
            {
                Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
                Gizmos.color = new Color(
                    1f,
                    (float)v / pipeSegCount,
                    (float)u / curveSegCount);
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
        
    }
    */

    private void SetVertices()
    {
        verts = new Vector3[pipeSegCount * curveSegCount * 4];

        float uStep = ringDistance / curveRadius;
        CreateFirstQuadRing(uStep);

        int iDelta = pipeSegCount * 4;

        for (int u = 2, i = iDelta; u <= curveSegCount; u++, i += iDelta)
        {
            CreateQuadRing(u * uStep, i);
        }
        
        mesh.vertices = verts;
    }

    private void CreateQuadRing(float u, int i)
    {
        float vStep = (2f * Mathf.PI) / pipeSegCount;
        int ringOffset = pipeSegCount * 4;

        Vector3 vertex = GetPointOnTorus(u, 0f);

        for (int v = 1; v <= pipeSegCount; v++, i += 4)
        {
            verts[i] = verts[i - ringOffset + 2];
            verts[i + 1] = verts[i - ringOffset + 3];
            verts[i + 2] = vertex;
            verts[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
        }
    }

    private void CreateFirstQuadRing(float u)
    {
        float vStep = (2f * Mathf.PI) / pipeSegCount;

        Vector3 vertexA = GetPointOnTorus(0f, 0f);
        Vector3 vertexB = GetPointOnTorus(u, 0f);

        for (int v = 1, i = 0; v <= pipeSegCount; v++, i += 4)
        {
            verts[i] = vertexA;
            verts[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
            verts[i + 2] = vertexB;
            verts[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
        }
    }

    private void SetTriangles()
    {
        triangles = new int[pipeSegCount * curveSegCount * 6];

        for (int t = 0, i = 0; t <triangles.Length; t += 6, i += 4)
        {
            triangles[t] = i;
            triangles[t + 1] = triangles[t + 4] = i + 1;
            triangles[t + 2] = triangles[t + 3] = i + 2;
            triangles[t + 5] = i + 3;
        }

        mesh.triangles = triangles;
    }

}
