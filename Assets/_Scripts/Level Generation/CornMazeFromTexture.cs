using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CornMazeFromTexture : MonoBehaviour
{
    public Texture2D texture;
    public int chunkSize = 10;

    public GameObject prefab;

    public List<GameObject> chunkObjects;

    [Button]
    public void Clear() 
    {
        if (chunkObjects != null)
        { 
            for (int i = 0; i < chunkObjects.Count; i++)
            {
                DestroyImmediate(chunkObjects[i]);
            }

            chunkObjects.Clear();
        }
    }

    [Button]
    public void GenerateMapFromTexture()
    {
        Clear();

        chunkObjects = new List<GameObject>();

        for (int x = 0; x < texture.width; x++) 
        {
            for (int z = 0; z < texture.height; z++)   
            {
                Color color = texture.GetPixel(x, z);   
                //float diff = Mathf.Abs(color.r - 1) + Mathf.Abs(color.g - 1) + Mathf.Abs(color.b - 1); 
                //Debug.Log(color + "  X:" + x + " Y: " + z);   
                if (color == Color.black)       
                {   
                    GameObject chunk = Instantiate(prefab); 
                      
                    chunk.transform.SetParent(transform);
                    chunk.transform.localScale = prefab.transform.lossyScale; 
                    chunk.transform.localPosition = new Vector3((x * 10) , 0, (z * 10f));
                    chunk.transform.localEulerAngles = Vector3.zero;
                    chunk.gameObject.SetActive(true);
                    chunkObjects.Add(chunk);

                } 
            }
        }
    }
}
