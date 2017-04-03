using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravObject : MonoBehaviour {
    private LatticeBehavior lattice;
    public float mass = 5;
    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("Lattice");
        lattice = (LatticeBehavior)go.GetComponent(typeof(LatticeBehavior));
        //lattice.deform(GetInstanceID(), this.gameObject.transform.position, mass);
    }
	
	// Update is called once per frame
	void Update () {
        lattice.deform(GetInstanceID(), this.gameObject.transform.position, mass);
	}
}
