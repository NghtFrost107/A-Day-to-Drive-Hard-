using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public const int height = 100;
    public const int width = 200;

    int lowestPoint;

    public Square[] boundary;

    public string seed;
    public bool useRandomSeed;

    public void GenerateMap(Square connectingSquare)
    {
        boundary = new Square[width];

        if (connectingSquare == null)
        {
            HeightGenerator(lowestPoint = 20);
            connectingSquare = boundary[0];
        } else
        {
            HeightGenerator(lowestPoint = connectingSquare.y);
        }
        Smoother();
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(boundary, lowestPoint, connectingSquare);

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
        
        for (int x = 0; x < boundary.Length; x++)
        {
            if(previousHeight == 0)
            {
                nextHeight = previousHeight + 1;
            }
            else if(previousHeight == height - 1)
            {
                nextHeight = previousHeight - 1;
            }else if (x == 0 || x >= boundary.Length - 2)
            {
                nextHeight = previousHeight;
            }
            else
            {
                if (targetHeight <= previousHeight)
                {                    
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

            boundary[x] = new Square(x, nextHeight);
            previousHeight = nextHeight;
        }        
    }

    void Smoother()
    {
        for (int i = 0; i < boundary.Length - 2; i++)
        {
            if (boundary[i].y == boundary[i + 2].y )
            {                
                if (boundary[i + 1].y < boundary[i].y)
                {
                    boundary[i + 1].y += 1;
                } else if (boundary[i + 1].y > boundary[i].y)
                {
                    boundary[i + 1].y -= 1;
                }

            }
        }
    }
}
