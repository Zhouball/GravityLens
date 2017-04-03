using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
    public float speed = 0.1f;
    public float min = 1;
    public float max = 15;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale += new Vector3(speed, speed, speed);
        if (this.transform.localScale.x >= max)
        {
            this.transform.localScale = new Vector3(min, min, min);
        }
	}
}
