using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationHandler : MonoBehaviour {

    static int sectionNumber = 1;

    int x = 0;

    MapGenerator section1;
    MapGenerator section2;

    public GameObject car;
    public GameObject carCamera;

    // Use this for initialization
    void Start () {
        section1 = transform.GetChild(0).gameObject.GetComponent<MapGenerator>();
        section2 = transform.GetChild(1).gameObject.GetComponent<MapGenerator>();
        section1.GenerateMap(20, x);
        x += MapGenerator.width;


        if (car != null)
        {
            car = Instantiate(car, section1.boundarySquares[1].topRight.position + new Vector3(2, 2, 0), Quaternion.identity);
            carCamera.GetComponent<CameraFollow>().target = car.transform.GetChild(0);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
