using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornBlock
{
    public static Vector2 GetUV(int index) 
    { 
        Vector2 uv = UVs[index];
        return uv;
    }


    public static Vector3 GetNormals(int index, bool cross = false)
    {
        Vector3 normal = new Vector3();

        if (!cross)
        {
            if (index <= 5)
                normal = Vector3.forward;
            else if (index <= 11)
                normal = Vector3.right;
            else if (index <= 17)
                normal = Vector3.left;
            else if (index <= 23)
                normal = -Vector3.forward;
        }
        else
        {
            return (Vector3.left + Vector3.right).normalized;
        }

        return normal;
    }


    //Triangle/UV indexes that make up each face.
    public static int[] front = { 0, 5 };
    public static int[] right = { 6, 11 };
    public static int[] left = { 12, 17 };
    public static int[] back = {18,23 };

    public static int[] triangles =
    {
        0, 2, 1, //face front
		0, 3, 2,
        1, 2, 5, //face right
		1, 5, 6,
        0, 7, 4, //face left
		0, 4, 3,
        5, 4, 7, //face back
		5, 7, 6,
    };

    public static Vector2[] UVs =
    {
        new Vector2(0.0f, 0f), //Front
        new Vector2(1f, 1.0f),
        new Vector2(1f, 0f),
        new Vector2(0.0f, 0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1f, 1.0f),
        new Vector2(0.0f, 0f), //Right
        new Vector2(0.0f, 1.0f),
        new Vector2(1f, 1.0f),
        new Vector2(0.0f, 0f),
        new Vector2(1f, 1.0f),
        new Vector2(1f, 0f),
        new Vector2(1f, 0f), //Left
        new Vector2(0.0f, 0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1f, 0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1f, 1.0f),
        new Vector2(1f, 1.0f), //Back
        new Vector2(0.0f, 1.0f),
        new Vector2(0.0f, 0f),
        new Vector2(1f, 1.0f),
        new Vector2(0.0f, 0f),
        new Vector2(1f, 0f),
    };

    public static Vector3[] vertices =
    {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1),
            new Vector3(0,0,1)
    };
}
