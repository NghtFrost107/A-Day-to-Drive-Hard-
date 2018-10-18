using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public const int height = 100;
    public const int width = 200;

    int lowestPoint;
    int xOffset = 0;

    public Square[] boundary;
    public List<GameObject> obstaclesSpawned;

    public string seed;
    public bool useRandomSeed;

    /*
     * Handles the creation of the terrain. Including determining the terrain shape, creating meshes and applying collision points
     */
    public void GenerateMap(Square connectingSquare)
    {
        boundary = new Square[width];

        if (connectingSquare == null)
        {
            HeightGenerator(lowestPoint = 20);
            connectingSquare = boundary[0];
        } else
        {
            xOffset = connectingSquare.x + 1;
            HeightGenerator(lowestPoint = connectingSquare.y);
        }
        Smoother();
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(boundary, lowestPoint, connectingSquare);

        SectionCollider addCollider = GetComponent<SectionCollider>();
        addCollider.addCollider();

    }

    /*
     * Determines the height for each boundary square that makes up the surface of the terrain
     * 
     * Takes into account the previous squares height to determine and appropriate height for the next square
     */
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

            boundary[x] = new Square(x + xOffset, nextHeight);
            previousHeight = nextHeight;
            
        }        
    }
    
    /*
     * Smooths the terrain to get rid of small jagged points
     */
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

    public void addObstacles(GameObject[] obstacles)
    {
        System.Random random = new System.Random();
        

        obstaclesSpawned = new List<GameObject>();
        for (int i = 0; i < 20; i++) {
            GameObject obstacleToSpawn = obstacles[random.Next(obstacles.Length)];
            int squareIndex = random.Next(1, boundary.Length - 1);
            if (boundary[squareIndex - 1].y == boundary[squareIndex + 1].y)
            {
                obstaclesSpawned.Add(Instantiate(obstacleToSpawn, boundary[squareIndex].topLeft.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity));
            }
            
        }
    }

    public void addClouds(Sprite[] clouds)
    {
        System.Random random = new System.Random();

        for(int i = 0; i < random.Next(50);i++)
        {
            GameObject cloud = new GameObject("Cloud");
            cloud.AddComponent<SpriteRenderer>().sprite = clouds[random.Next(clouds.Length - 1)];
            cloud.transform.position = boundary[random.Next(1, boundary.Length - 1)].topLeft.position + new Vector3(0, random.Next(10,20), 0);
            obstaclesSpawned.Add(cloud);
        }
    }


    public void removeAllObstacles()
    {
        foreach(GameObject obstacle in obstaclesSpawned)
        {
            Destroy(obstacle);
        }
    }
}
