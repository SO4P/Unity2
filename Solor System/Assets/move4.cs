using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move4 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.parent.position, new Vector3(1/10, 1, 0), 60 * Time.deltaTime);
    }
}
