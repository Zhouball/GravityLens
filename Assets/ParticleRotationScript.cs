using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotationScript : MonoBehaviour {

	private Transform center;
	private Quaternion angularVel;
	[SerializeField] public float omega;


	// Use this for initialization
	void Start () {
		center = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		angularVel = Quaternion.AngleAxis (omega * Time.deltaTime, new Vector3 (0.0f, 0.0f, 1.0f));
		center.rotation = center.rotation * angularVel;
	}
}
