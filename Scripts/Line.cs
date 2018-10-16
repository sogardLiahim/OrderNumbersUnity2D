using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    private LineRenderer lr;
    int i = 0;

	// Use this for initialization
	void Start ()
    {

        lr = GetComponent<LineRenderer>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        i++;
        lr.SetPosition(1, new Vector3(0, i, 0) * Time.deltaTime);
		
	}
}
