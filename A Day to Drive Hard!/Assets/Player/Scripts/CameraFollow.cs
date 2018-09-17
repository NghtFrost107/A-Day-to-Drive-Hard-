using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update ()
    {
		if(target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.2f, 0.5f, point.z)); // (new Vector(0.5, 0.5, point.z)
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 0);
        }
	}
}
