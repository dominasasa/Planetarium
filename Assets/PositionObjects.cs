using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PositionObjects : MonoBehaviour
{


	void Awake()
	{
		GameObject[] lights = new GameObject[30];
		GameObject ob = GameObject.FindWithTag("Light");

		for(int i = 0; i < 30; i++)
		{
			float radius = 20;
			float theta = Random.Range(0, 360) * Mathf.Deg2Rad;
			float phi = Random.Range(0, 360) * Mathf.Deg2Rad;

			float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
			float y = radius * Mathf.Cos(phi);
			float z = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
			
			GameObject obj = Instantiate(ob, new Vector3(x,y,z),Quaternion.identity);
			
			Component halo = obj.GetComponent("Halo");

			halo.GetType().GetProperty("enabled").SetValue(halo, true,null);
		
			obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			obj.gameObject.tag = "Light";

			lights[i] = obj;

		}

		GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Star");
		

		foreach (var sphere in allSpheres)
		{
			float radius = 20;
			float theta = Random.Range(0,360) * Mathf.Deg2Rad;
			float phi = Random.Range(0,360) * Mathf.Deg2Rad;

			float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
			float y = radius * Mathf.Cos(phi);
			float z = radius * Mathf.Sin(theta) * Mathf.Cos(phi);

			sphere.transform.localScale  = new Vector3(0.1f,0.1f,0.1f);
			
			sphere.transform.position = new Vector3(x, y, z);
		}
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
