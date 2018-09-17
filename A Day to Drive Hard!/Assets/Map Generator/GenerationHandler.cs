using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationHandler : MonoBehaviour {

    static int sectionNumber = 1; //Increments as each new section is generated

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


        if (car != null)
        {
            car.transform.position = section1.boundary[10].topRight.position + new Vector3(2, 3, 0);
            //car = Instantiate(car, new Vector3(5,-13,0), Quaternion.identity); //- USE FOR CONTINUOUS GENERATION TESTING PURPOSES

            carCamera.GetComponent<CameraFollow>().target = car.transform.GetChild(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (s2Generated == false) //If the new section hasn't been generated yet (Player is on the first section object and it needs to generate the second section)
        {
            if (car.transform.GetChild(0).transform.position.x > section1.transform.position.x + 75) //When the player has moved 75 squares into the current section
            {
                if (sectionNumber != 1) {
                    section2.GetComponent<MeshGenerator>().ClearMesh(); //Delete old section
                }
                section2.transform.position = new Vector3(MapGenerator.width * sectionNumber, 0, 0);
                section2.GenerateMap(section1.boundary[section1.boundary.Length - 1]); //Create new section 
                s2Generated = true;
                sectionNumber += 1;
            }
        } else //If the player is on the second section object and it needs to generate the next section
        {
            if (car.transform.GetChild(0).transform.position.x > section2.transform.position.x + 75)
            {
                section1.GetComponent<MeshGenerator>().ClearMesh();
                section1.transform.position = new Vector3(MapGenerator.width * sectionNumber, 0, 0);
                section1.GenerateMap(section2.boundary[section2.boundary.Length - 1]);
                s2Generated = false;
                sectionNumber += 1;
            }

        }
	}
}
