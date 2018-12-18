using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;
using SQLite4Unity3d;
using Random = UnityEngine.Random;

public static class Constants
{
	public static SQLite4Unity3d.SQLiteConnection Dbcon;
	public static float MeanHeading;
}

public class PositionObjects : MonoBehaviour
{
	void Awake()
	{
		
	}

	private static void LoadDb()
	{
		// check if file exists in Application.persistentDataPath

		string filepath = Application.persistentDataPath + "/" + "Planetarium.db";

		if (!File.Exists(filepath))

		{
			// if it doesn't ->

			// open StreamingAssets directory and load the db ->

			WWW loadDb =
				new WWW("jar:file://" + Application.dataPath + "!/assets/" +
				        "Planetarium.db"); // this is the path to your StreamingAssets in android

			while (!loadDb.isDone)
			{
			} // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

			// then save to Application.persistentDataPath

			File.WriteAllBytes(filepath, loadDb.bytes);
		}

		//open db connection

		var connection = "URI=file:" + filepath;

		Constants.Dbcon = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

		Debug.Log("Db connection opened");
	}

	private static void CreateMajorObjects()
	{
		Debug.Log("CreateMajorObjects");
		GameObject planet = GameObject.FindWithTag("Planet");

		GameObject moon = GameObject.FindWithTag("Moon");

		GameObject star = GameObject.FindWithTag("Star");


		IEnumerable<Major> majorObjects = Constants.Dbcon.Table<Major>();

		foreach (var majorObject in majorObjects)
		{
			if (majorObject.type == "planet")
			{
				Debug.Log("Creating object: " + majorObject.name);

				var position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(Input.location.lastData.longitude,
					Input.location.lastData.latitude, majorObject.name);

				float x, y, z;

				SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

				GameObject obj = Instantiate(planet, new Vector3(x, y, z), Quaternion.identity);

				obj.transform.localScale = new Vector3(5f, 5f, 5f);
			}

			if (majorObject.type == "moon")
			{
				Debug.Log("Creating object: " + majorObject.name);
				Vector3 position = new Vector3();
				if (majorObject.name == "Moon")
				{
					position =
						CelestialCoordinates.CalculateHorizontalCoordinatesMoon(Input.location.lastData.longitude,
							Input.location.lastData.latitude);
				}
				else if (!string.IsNullOrEmpty(majorObject.rot_pole_ra) && !string.IsNullOrEmpty(majorObject.rot_pole_de))
				{
					position = CelestialCoordinates.CalculateHorizontalCoordinatesStar(Input.location.lastData.longitude,
						Input.location.lastData.latitude, float.Parse(majorObject.rot_pole_ra), float.Parse(majorObject.rot_pole_de));
				}

				float x, y, z;

				SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

				GameObject obj = Instantiate(moon, new Vector3(x, y, z), Quaternion.identity);

				obj.transform.localScale = new Vector3(5f, 5f, 5f);
			}
		}

		//GameObject[] lights = new GameObject[250];

		//for (int i = 0; i < 250; i++)
		//{
		//	float x1, y1, z1;
		//	SphericalToCartesian(out x1, out y1, out z1);


		//	GameObject obj = Instantiate(star, new Vector3(x1, y1, z1), Quaternion.identity);

		//	obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

		//	obj.gameObject.tag = "Star";

		//	lights[i] = obj;
		//}
	}

	private static void SphericalToCartesian(out float x, out float y, out float z)
	{
		float radius = 20;
		float theta = Random.Range(0, 360) * Mathf.Deg2Rad;
		float phi = Random.Range(0, 180) * Mathf.Deg2Rad;

		x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
		y = radius * Mathf.Cos(phi);
		z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);
	}

    // Use this for initialization
    void Start()
    {
        while (Input.location.status != LocationServiceStatus.Running)
        {
        }

        LoadDb();

        Vector3 position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(Input.location.lastData.longitude,
            Input.location.lastData.latitude, "Mercury");

        GameObject mercury = GameObject.FindGameObjectWithTag("Mercury");

        float sumHeading = 0;
        UInt64 kl = 0;
        while (Constants.MeanHeading <= 0 && kl < 5000)
		{
			for (int i = 0; i < 10000; i++)
			{
				sumHeading += Input.compass.trueHeading;
			}

            Debug.Log(kl++);
			Constants.MeanHeading = sumHeading / 10000;
		}

		Debug.Log("meanHeading: " + Constants.MeanHeading);

		float x, y, z;
		SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

		mercury.transform.position = new Vector3(x, y, z);

		print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " +
		      Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " +
		      Input.location.lastData.timestamp);


		CreateMajorObjects();
	}

	private static void SphericalToCartesianFromPosition(out float x, out float y, out float z, float meanHeading,
		Vector3 position)
	{
		float radius = 100;
		float theta = (360 - meanHeading + position.y) * Mathf.Deg2Rad;
		float phi = position.x * Mathf.Deg2Rad;

		x = radius * Mathf.Cos(phi) * Mathf.Sin(theta);
		y = radius * Mathf.Sin(phi);
		z = radius * Mathf.Cos(theta) * Mathf.Cos(phi);
	}


	// Update is called once per frame
	void Update()
	{
	}

	
}