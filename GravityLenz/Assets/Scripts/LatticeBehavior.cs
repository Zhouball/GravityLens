using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeBehavior : MonoBehaviour {

    public GameObject GlowPoint;
    public int arrayX = 16;
    public int arrayY = 16;
    public int arrayZ = 16;
    public float gravity = 0.5f;
    public float elasticity = 0.8f;
    public float mtug = 0.3f;

    private Point[,,] points;

	// Use this for initialization
	void Start () {
        points = new Point[arrayX,arrayY,arrayZ];
        Point point;
        Vector3 pos;
        for (int x = 0; x < arrayX; x++)
        {
            for (int y = 0; y < arrayY; y++)
            {
                for (int z = 0; z < arrayZ; z++)
                {
                    point = new Point(Instantiate(GlowPoint, this.gameObject.transform));
                    point.GlowPoint.transform.position += new Vector3(x, y, z);
                    point.saveOrigin();
                    points [x,y,z] = point;
                }
            }
        }

        //deform(0, new Vector3(3 + 3 * Mathf.Sin(Time.time * 0.5f), 3 + 5 * Mathf.Sin(Time.time * 0.5f), 3 + 2 * Mathf.Sin(Time.time * 0.5f)), 1);
    }
	
	void Update () {
        //Debug.Log(points[0,0,0].forces.Count);
        for (int x = 0; x < arrayX; x++)
        {
            for (int y = 0; y < arrayY; y++)
            {
                for (int z = 0; z < arrayZ; z++)
                {
                    points[x, y, z].despos = points[x, y, z].oripos;
                    for (int i = 0; i < points[x, y, z].forces.Count; i++)
                    {
                        points[x, y, z].despos += points[x, y, z].forces[i].getForce();
                    }
                    //Vector3 tug = points[x, y, z].despos - points[x, y, z].oripos;
                    //float distance = tug.magnitude;
                    //tug.Normalize();
                    //tug *= distance;
                    //tug *= mtug;
                    //points[x, y, z].despos += tug;
                    points[x, y, z].GlowPoint.transform.position = Vector3.Lerp(points[x, y, z].GlowPoint.transform.position, points[x, y, z].despos, elasticity);

                    //points[x, y, z].GlowPoint.transform.position = points[x, y, z].despos;
                }
            }
        }
        //deform(0, new Vector3(3 + 3 * Mathf.Sin(Time.time * 0.5f), 3 + 5 * Mathf.Sin(Time.time * 0.5f), 3 + 2 * Mathf.Sin(Time.time * 0.5f)), 1);
        //deform(0, new Vector3(3 + 3 * Mathf.Sin(Time.time * 0.5f), 3 + 5 * Mathf.Sin(Time.time * 0.5f), 3 + 2 * Mathf.Sin(Time.time * 0.5f)), 1);
    }

    private class Point
    {
        public GameObject GlowPoint;
        public Vector3 oripos;
        public Vector3 despos;
        public List<Deformation> forces = new List<Deformation>();

        public Point(GameObject point)
        {
            GlowPoint = point;
            despos = oripos;
        }
        public void saveOrigin()
        {
            oripos = GlowPoint.transform.position;
        }
    }

    private class Deformation
    {
        public int id;
        Vector3 frc;
        public void setForce(Vector3 force)
        {
            frc = force;
        }
        public Vector3 getForce()
        {
            return frc;
        }
        public Deformation(int i, Vector3 force)
        {
            id = i;
            frc = force;
        }
    }

    public void deform (int id, Vector3 position, float mass)
    {
        Vector3 force;
        Deformation deformation;
        for (int x = 0; x < arrayX; x++)
        {
            for (int y = 0; y < arrayY; y++)
            {
                for (int z = 0; z < arrayZ; z++)
                {   
                    force = position - points[x, y, z].oripos;
                    float sqrdistance = force.sqrMagnitude;
                    if (sqrdistance <= 3f)
                    {
                        sqrdistance = 3f;
                    }
                    force.Normalize();
                    force *= mass;
                    force *= gravity;
                    force /= sqrdistance;
                    bool exists = false;
                    for (int i = 0; i < points[x, y, z].forces.Count; i++)
                    {
                        if (points[x, y, z].forces[i].id == id)
                        {
                            //Debug.Log("Gu");
                            points[x, y, z].forces[i].setForce(force);
                            exists = true;
                        }
                    }
                    if (exists) continue;
                    deformation = new global::LatticeBehavior.Deformation(id, force);
                    points[x, y, z].forces.Add(deformation);
                }
            }
        }
    }
}
