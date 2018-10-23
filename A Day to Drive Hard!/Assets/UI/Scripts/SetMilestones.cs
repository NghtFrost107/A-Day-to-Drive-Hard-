using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMilestones : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("Milestone").GetComponent<MilestonesManager>().DisplayMilestones(gameObject);
	}
	
}
