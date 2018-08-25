﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    int height = Screen.height;
    int width = Screen.width;

    Square[,] section;

    string seed;

    void Start()
    {
        GenerateMap();
        
    }
    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
	}

    void GenerateMap()
    {
        section = new Square[width, height];
        HeightGenerator();

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(section);
    }

    void HeightGenerator()
    {
        System.Random random = new System.Random();

        int nextHeight = random.Next(0, height -1);
        int targetHeight = random.Next(0, height - 1);

        int previousHeight = nextHeight;
        
        for (int x = 0; x < width; x++)
        {
            if(previousHeight == 0)
            {
                nextHeight = previousHeight + 1;
            }
            else if(previousHeight == height - 1)
            {
                nextHeight = previousHeight - 1;
            }
            else
            {
                //   nextHeight = random.Next(previousHeight - 1, previousHeight + 2); //Doesn't return the max value so must do +2 to make it pick 1 up the previous height
                if (targetHeight <= previousHeight)
                {
                    int heightFromTarget = previousHeight - targetHeight;
                    
                    if (x < width / 2)
                    {
                        nextHeight = random.Next(previousHeight - 1, previousHeight + 2);
                    } else if (x < width / 4)
                    {
                        nextHeight = random.Next(0, 100) < 55 ? previousHeight - 1 : previousHeight + 1;
                    } else if (x < width / 8)
                    {
                        nextHeight = random.Next(0, 100) < 65 ? previousHeight - 1 : previousHeight + 1;
                    } else
                    {
                        nextHeight = random.Next(0, 100) < 75 ? previousHeight - 1 : previousHeight + 1;
                    }
                } else
                {
                    int heightFromTarget = targetHeight - previousHeight;

                    if (x < width / 2)
                    {
                        nextHeight = random.Next(previousHeight - 1, previousHeight + 2);
                    }
                    else if (x < width / 4)
                    {
                        nextHeight = random.Next(0, 100) < 55 ? previousHeight + 1 : previousHeight - 1;
                    }
                    else if (x < width / 8)
                    {
                        nextHeight = random.Next(0, 100) < 65 ? previousHeight + 1 : previousHeight - 1;
                    }
                    else
                    {
                        nextHeight = random.Next(0, 100) < 75 ? previousHeight + 1 : previousHeight - 1;
                    }
                }

            }
            
            for (int y = 0; y < height; y++)
            {
                if (y < nextHeight)
                {
                    section[x, y] = new Square(1);
                } else if (y == nextHeight)
                {
                    section[x, y] = new Square(2);
                }
                else if (y >= nextHeight)
                {
                    section[x, y] = new Square(0);
                }
            }

            previousHeight = nextHeight;
        }
        
    }

    void FillYAxis(int rowX, int boundaryY)
    {

    }
}
