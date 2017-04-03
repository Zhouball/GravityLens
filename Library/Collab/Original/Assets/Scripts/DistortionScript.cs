using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionScript : MonoBehaviour {

	public GameObject MeshCube;
	public int totalCubeCount = 4096;
	public int PPC = 16;

	private GameObject[] MeshCubes;

	private LineRenderer[] currentFrameLineRenderers;
	private Vector3[] currentFramePoints;
	private int[] currentFramePointIndeces;
	private int currentFramePointCount; 

	public Vector3 origin = Vector3.zero;

	void Start () {
		currentFrameLineRenderers = new LineRenderer[totalCubeCount * PPC];
		currentFramePoints = new Vector3[totalCubeCount * PPC];
		currentFramePointIndeces = new int[totalCubeCount * PPC];
		currentFramePointCount = 0; 

		MeshCubes = new GameObject[totalCubeCount];

		for (int i = 0; i < totalCubeCount; i++) {
			MeshCubes [i] = Instantiate (MeshCube, this.gameObject.transform);

			int x0 = i & 1;
			int y0 = i & 2;
			int z0 = i & 4;
			int x1 = i & 8;
			int y1 = i & 16;
			int z1 = i & 32;
			int x2 = i & 64;
			int y2 = i & 128;
			int z2 = i & 256;
			int x3 = i & 512;
			int y3 = i & 1024;
			int z3 = i & 2048;

			LineRenderer lr = MeshCubes[i].GetComponent<LineRenderer> ();

			for (int j = 0; j < PPC; j++) {
				Vector3 point = lr.GetPosition (j);
				if (x0 != 0) {
					point.x += 1.0f;
				}
				if (y0 != 0) {
					point.y += 1.0f;
				}
				if (z0 != 0) {
					point.z += 1.0f;
				}
				if (x1 != 0) {
					point.x += 2.0f;
				}
				if (y1 != 0) {
					point.y += 2.0f;
				}
				if (z1 != 0) {
					point.z += 2.0f;
				}
				if (x2 != 0) {
					point.x += 4.0f;
				}
				if (y2 != 0) {
					point.y += 4.0f;
				}
				if (z2 != 0) {
					point.z += 4.0f;
				}
				if (x3 != 0) {
					point.x += 8.0f;
				}
				if (y3 != 0) {
					point.y += 8.0f;
				}
				if (z3 != 0) {
					point.z += 8.0f;
				}
				lr.SetPosition (j, point);
			}
		}
	}

	void Update () {
		reset ();
        // origin = Vector3.Lerp (origin, new Vector3 (40.0f, 40.0f, 40.0f), Time.deltaTime * 0.1f);
        origin = new Vector3(5 + 3 * Mathf.Sin(Time.time * 0.5f), 5 + 5 * Mathf.Sin(Time.time * 0.5f), 5 + 2 * Mathf.Sin(Time.time * 0.5f));
		distort (origin, 4.0f);
	}

	public void distort (Vector3 origination, float threshhold) {
		bool isFirst = false;
		if (currentFramePointCount == 0) {
			isFirst = true;
		}

		foreach (GameObject cube in MeshCubes) {
			float sqrThreshhold = threshhold * threshhold;
			LineRenderer lr = cube.GetComponent<LineRenderer> ();
			for (int i = 0; i < PPC; i++) {
				Vector3 worldPoint = lr.GetPosition (i);
				Vector3 dir = worldPoint - origination;
				if (dir.sqrMagnitude <= sqrThreshhold) {
					if (isFirst) {
						currentFrameLineRenderers [currentFramePointCount] = lr;
						currentFramePointIndeces [currentFramePointCount] = i;
						currentFramePoints [currentFramePointCount] = worldPoint;
						currentFramePointCount++;
					}
				
					float percentage = (dir.magnitude / sqrThreshhold);
					Vector3 newPoint = (dir * percentage) + origination;
					lr.SetPosition (i, newPoint);
					Color mat = lr.material.color;
					mat.a = 1.0f - percentage;
					lr.material.color = mat;
				}
			}
		}
	}

	private void reset () {
		for (int i = 0; i < currentFramePointCount; i++) {
			currentFrameLineRenderers [i].SetPosition (currentFramePointIndeces [i], currentFramePoints [i]);
		}

		foreach (GameObject cube in MeshCubes) {
			LineRenderer lr = cube.GetComponent <LineRenderer> ();
			lr.material = MeshCube.GetComponent <LineRenderer> ().sharedMaterial;
		}

		currentFramePointCount = 0;
		currentFrameLineRenderers = new LineRenderer[totalCubeCount * PPC];
		currentFramePoints = new Vector3[totalCubeCount * PPC];
		currentFramePointIndeces = new int[totalCubeCount * PPC];
	}
}
