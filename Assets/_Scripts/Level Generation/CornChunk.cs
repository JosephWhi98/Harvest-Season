using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class CornChunk : MonoBehaviour
{
    public int chunkSize = 16; //16x16

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Mesh mesh;
    public MeshCollider meshCollider; 

    public int[,] positions;

    public void Awake()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mesh = meshFilter.mesh; 

        GeneratePositions();
    }

    public void GeneratePositions()
    {
        positions = new int[chunkSize, chunkSize];

        for (int x = 0;  x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                positions[x, z] = 1;
            } 
        }

        GenerateChunk();
    }

    public void GenerateChunk()
    {
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();
        List<int> mainMeshTriangles = new List<int>();
         
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                List<int> neededVerts = new List<int>(); // CornBlock.front.Concat(CornBlock.back).Concat(CornBlock.left).Concat(CornBlock.right).ToList();

                foreach(int v in CornBlock.triangles)
                    neededVerts.Add(v); 

                foreach (int i in neededVerts) 
                { 
                    Debug.Log(i);
                    uvs.Add(CornBlock.GetUV(i));
                    vertices.Add(CornBlock.vertices[i] + new Vector3Int(x, 0, z));
                    mainMeshTriangles.Add(vertices.Count - 1);
                }
            }
        }

        //Build the mesh
        mesh.Clear();
        mesh.SetVertices(vertices.ToArray());
        mesh.SetTriangles(mainMeshTriangles.ToArray(), 0);
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh; 
    }
}
