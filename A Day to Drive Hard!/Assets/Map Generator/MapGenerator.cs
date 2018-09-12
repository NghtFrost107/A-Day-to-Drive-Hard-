using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public const int height = 100;
    public const int width = 200;

    int lowestPoint;

    public Square[] boundarySquares;

    public string seed;
    public bool useRandomSeed;

    public void GenerateMap(Square connectingSquare)
    {
        boundarySquares = new Square[width];


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
        meshGen.GenerateMesh(boundarySquares, lowestPoint, connectingSquare);

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

            boundarySquares[x] = new Square(x, nextHeight);
            previousHeight = nextHeight;
        }
        
    }

    void Smoother()
    {
        for (int i = 0; i < boundarySquares.Length - 2; i++)
        {
            if (boundarySquares[i].y == boundarySquares[i + 2].y )
            {                
                if (boundarySquares[i + 1].y < boundarySquares[i].y)
                {
                    boundarySquares[i + 1].y += 1;
                } else if (boundarySquares[i + 1].y > boundarySquares[i].y)
                {
                    boundarySquares[i + 1].y -= 1;
                }

            }
        }
    }
}
