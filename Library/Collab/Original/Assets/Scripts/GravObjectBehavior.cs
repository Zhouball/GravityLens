using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravObjectBehavior : MonoBehaviour {
    private GameObject MeshWorld;
    private DistortionScript script;
	// Use this for initialization
	void Start () {
        MeshWorld = GameObject.Find("MeshWorld");
        script = (DistortionScript)MeshWorld.GetComponent(typeof(DistortionScript));
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.gameObject.transform.position);
        script.distort(this.gameObject.transform.position, 5.0f);
    }
}
