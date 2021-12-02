using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//maybe use a seed

public class LevelFactory : MonoBehaviour
{
    // Start is called before the first frame update
    private int width;
    private int height;
    private int depth;
    private int quadCount;
    void Start()
    {

    }
}

class Quad
{
    private int width;
    private int height;
    private int depth;
    private Vector3 pylon;
    private Vector3[] boxes;
    private Vector3[] zergSpawns;
    private Vector3[] hunterSpawns;
    public Quad(int initWidth, int initHeight, int initDepth)
    {
        width = initWidth;
        height = initHeight;
        depth = initDepth;
    }
    public void InitQuad()
    {
        Debug.Log("new piece");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Debug.Log(boxes[i]);
            }
        }
    }
}
