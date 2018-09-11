﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public const int height = 100;
    public const int width = 200;

    int lowestPoint;

    Square[,] section;
    public Square[] boundarySquares;

    public string seed;
    public bool useRandomSeed;

    public void GenerateMap(Square connectingSquare)
    {
        section = new Square[width, height];
        boundarySquares = new Square[section.GetLength(0)];

        for (int x = 0; x < section.GetLength(0); x++)
        {
            for (int y = 0; y < section.GetLength(1); y++) {
                section[x, y] = new Square(x, y);
            }
        }

        if (connectingSquare == null)
        {
            HeightGenerator(lowestPoint = 20);
            connectingSquare = boundarySquares[0];
        } else
        {
            HeightGenerator(lowestPoint = connectingSquare.y);
        }
        Smoother();
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(section, lowestPoint, connectingSquare);

        SectionCollider addCollider = GetComponent<SectionCollider>();
        addCollider.addCollider();

    }

    void HeightGenerator(int lowestPoint)
    {
        if (useRandomSeed)
        {
            //seed = 
        }
        System.Random random = new System.Random();

        int nextHeight = lowestPoint;
        int targetHeight = random.Next(0, height/2);

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
                if (targetHeight <= previousHeight)
                {
                    //int heightFromTarget = previousHeight - targetHeight;
                    
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
                    //int heightFromTarget = targetHeight - previousHeight;

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
                    section[x, y].state = (int)StateTypes.TERRAIN;
                } else if (y == nextHeight)
                {
                    if (lowestPoint > section[x,y].y)
                    {
                        lowestPoint = section[x,y].y;
                    }
                    section[x, y].state = (int)StateTypes.BOUNDARY;
                    boundarySquares[x] = section[x, y];
                }
                else if (y >= nextHeight)
                {
                    section[x, y].state = (int)StateTypes.AIR;
                }
            }

            previousHeight = nextHeight;
        }
        
    }

    void Smoother()
    {
        for (int i = 0; i < boundarySquares.Length - 2; i++)
        {
            if (boundarySquares[i].y == boundarySquares[i + 2].y )
            {
                Square nextBoundarySquare = FindBoundarySquare(i + 1);
                
                if (nextBoundarySquare.y < boundarySquares[i].y)
                {
                    nextBoundarySquare.state = 1;
                    boundarySquares[i + 1] = section[i + 1, boundarySquares[i + 1].y + 1];
                } else if (nextBoundarySquare.y > boundarySquares[i].y)
                {
                    nextBoundarySquare.state = 0;
                    boundarySquares[i + 1] = section[i + 1, boundarySquares[i + 1].y - 1];
                }
                boundarySquares[i + 1].state = 2;

            }
        }
    }

    public Square FindBoundarySquare(int xColumn)
    {
        for (int y = 0; y < section.GetLength(1); y++)
        {
            if (section[xColumn, y].state == 2)
            {
                return section[xColumn, y];
            }
        }

        return null;
    }
}
