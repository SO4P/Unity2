using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move7 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.parent.position, new Vector3(1/2, 1, 0), 30 * Time.deltaTime);
    }
}
