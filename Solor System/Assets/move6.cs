using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move6 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.parent.position, new Vector3(1/5, 1, 0), 40 * Time.deltaTime);
    }
}
