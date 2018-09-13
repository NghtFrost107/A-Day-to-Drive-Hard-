using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationHandler : MonoBehaviour {

    static int sectionNumber = 0;

    int x = 0;
    bool s2Generated = false;

    MapGenerator section1;
    MapGenerator section2;

    public GameObject car;
    public GameObject carCamera;

    // Use this for initialization
    void Start () {
        section1 = transform.GetChild(0).gameObject.GetComponent<MapGenerator>();
        section2 = transform.GetChild(1).gameObject.GetComponent<MapGenerator>();
        section1.GenerateMap(null);
        x += MapGenerator.width;


        if (car != null)
        {
            car = Instantiate(car, section1.boundary[10].topRight.position + new Vector3(2, 3, 0), Quaternion.identity);
            //car = Instantiate(car, new Vector3(5,-13,0), Quaternion.identity); //- USE FOR CONTINUOUS GENERATION TESTING PURPOSES
            carCamera.GetComponent<CameraFollow>().target = car.transform.GetChild(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (s2Generated == false)
        {
            if (car.transform.GetChild(0).transform.position.x > section1.transform.position.x + 75)
            {
                if (sectionNumber != 0) {
                    section2.GetComponent<MeshGenerator>().ClearMesh();
                }
                section2.transform.position = new Vector3(x, 0, 0);
                section2.GenerateMap(section1.boundary[section1.boundary.Length - 1]);
                x += MapGenerator.width;
                s2Generated = true;
                sectionNumber += 1;
            }
        } else
        {
            if (car.transform.GetChild(0).transform.position.x > section2.transform.position.x + 75)
            {
                section1.GetComponent<MeshGenerator>().ClearMesh();
                section1.transform.position = new Vector3(x, 0, 0);
                section1.GenerateMap(section2.boundary[section2.boundary.Length - 1]);
                x += MapGenerator.width;
                s2Generated = false;
                sectionNumber += 1;
            }

        }
	}
}
