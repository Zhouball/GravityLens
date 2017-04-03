using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsarBehavior : MonoBehaviour {
    private LatticeBehavior lattice;
    public float mass = 5;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("Lattice");
        lattice = (LatticeBehavior)go.GetComponent(typeof(LatticeBehavior));
        //GetComponent<Renderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            lattice.deform(i, vertices[i], mass);
        }
    }
}
