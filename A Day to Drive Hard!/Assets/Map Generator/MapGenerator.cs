using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    int height = Screen.height;
    int width = Screen.width;

    int[,] section;

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
        section = new int[width, height];
        RandomVoidFill();

    }

    void RandomVoidFill()
    {
        System.Random random = new System.Random();

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1)
                {
                    section[x, y] = 1;
                }
                else
                {
                    section[x, y] = random.Next(0, 100) > 50 ? 1 : 0;
                }
            }
        }
        
    }

    void OnDrawGizmos()
    {
        if (section != null)
        {
            Gizmos.color = Color.green;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (section[x, y] == 1)
                    {
                        Vector3 pos = new Vector3(-width / 2 + x, 0, -height / 2 + y);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }

                }
            }
        }
    }
}
