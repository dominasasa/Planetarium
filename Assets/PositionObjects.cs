using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PositionObjects : MonoBehaviour
{


	void Awake()
	{

		GameObject star = GameObject.FindWithTag("Star");

		float radius = 20;
		float theta = Random.Range(0, 360) * Mathf.Deg2Rad;
		float phi = Random.Range(0, 180) * Mathf.Deg2Rad;

		float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
		float y = radius * Mathf.Cos(phi);
		float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

		star.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

		star.transform.position = new Vector3(x, y, z);

		GameObject[] lights = new GameObject[250];
		
		for (int i = 0; i < 250; i++)
		{
			float radius1 = 20;
			float theta1 = Random.Range(0, 360) * Mathf.Deg2Rad;
			float phi1 = Random.Range(0, 180) * Mathf.Deg2Rad;

			float x1 = radius1 * Mathf.Cos(theta1) * Mathf.Sin(phi1);
			float y1 = radius1 * Mathf.Cos(phi1);
			float z1 = radius1 * Mathf.Sin(theta1) * Mathf.Sin(phi1);

			GameObject obj = Instantiate(star, new Vector3(x1, y1, z1), Quaternion.identity);

			obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

			obj.gameObject.tag = "Star";

			lights[i] = obj;
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
