using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour {
    public byte[,,] map;

    public Mesh ChunkMesh;                                         
    protected MeshFilter meshFilter;                                 
    protected MeshRenderer meshRenderer;                        
    protected MeshCollider meshCollider;

    // Use this for initializations
    void Start()
    {

  
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
   
        map = new byte[World.Instance.chunkWidth, World.Instance.chunkHeight, World.Instance.chunkWidth];

        for (int x = 0; x < 20; x++)
        {

            for (int z = 0; z <20; z++)
            {

                map[x, 0, z] = 1;
                map[x, 1, z] = (byte)Random.Range(0, 1);
            }
        }
      
        CreateChunkMesh();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateChunkMesh()
    {
        Debug.Log("createchunkmesh");
        ChunkMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 20; z++)
                {
                    if (map[x, y, z] == 0) continue;
                    byte brick = map[x, y, z];
                    BuildFace(brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                }
            }
        }
        ChunkMesh.vertices = verts.ToArray();
        ChunkMesh.uv = uvs.ToArray();
        ChunkMesh.triangles = tris.ToArray();
        ChunkMesh.RecalculateBounds();
        ChunkMesh.RecalculateNormals();

        meshFilter.mesh = ChunkMesh;
        meshCollider.sharedMesh = ChunkMesh;
    }
    public virtual void BuildFace( byte brick,Vector3 corner,Vector3 up, Vector3 right, bool reversed,List<Vector3> verts, List<Vector2> uvs, List<int> tirs)
    {
        Debug.Log("buildface");
        int index = verts.Count;
        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        if (reversed)
        {
            // 1st Triangle
            tirs.Add(index + 0);
            tirs.Add(index + 1);
            tirs.Add(index + 2);

            // 2rd Triangle
            tirs.Add(index + 2);
            tirs.Add(index + 3);
            tirs.Add(index + 0);
        }
        else
        {
            // 1st Triangle
            tirs.Add(index + 1);
            tirs.Add(index + 0);
            tirs.Add(index + 2);

            // 2rd Triangle
            tirs.Add(index + 3);
            tirs.Add(index + 2);
            tirs.Add(index + 0);
        }
    }
}
