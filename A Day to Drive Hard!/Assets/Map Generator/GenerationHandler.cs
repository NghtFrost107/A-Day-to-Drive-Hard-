using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationHandler : MonoBehaviour {

    static int sectionNumber = 1;

    int x = 0;
    bool generated = false;

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
            car = Instantiate(car, section1.boundarySquares[1].topRight.position + new Vector3(2, 2, 0), Quaternion.identity);
            carCamera.GetComponent<CameraFollow>().target = car.transform.GetChild(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (car.transform.GetChild(0).transform.position.x > 20 && generated == false) {//(section1.transform.position.x - (x - MapGenerator.width/2))) {
            //section2.GetComponent<MeshGenerator>().ClearMesh();
            section2.transform.position = new Vector3(x, 0, 0);
            section2.GenerateMap(section1.boundarySquares[section1.boundarySquares.Length-1]);
            x += MapGenerator.width;
            generated = true;
        }
	}
}
