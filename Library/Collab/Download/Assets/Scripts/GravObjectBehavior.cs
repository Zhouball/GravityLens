using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravObjectBehavior : MonoBehaviour {
    
	private GameObject MeshWorld;
    private DistortionScript script;

	void Start () {
        MeshWorld = GameObject.Find ("MeshWorld");
		script = (DistortionScript) MeshWorld.GetComponent (typeof (DistortionScript));
    }
	
	void Update () {
        script.distort(this.gameObject.transform.position, 5.0f);
    }
}
