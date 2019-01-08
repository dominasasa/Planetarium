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
	public static IEnumerable<StarInfo> StarInfo;
	public static IEnumerable<MajorInfo> MajorInfo;
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

		//if (!File.Exists(filepath))

		//{
		// if it doesn't ->

		// open StreamingAssets directory and load the db ->

		WWW loadDb =
			new WWW("jar:file://" + Application.dataPath + "!/assets/" +
					"Planetarium.db"); // this is the path to your StreamingAssets in android

		while (!loadDb.isDone)
		{
		} //TODO: CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

		// then save to Application.persistentDataPath

		File.WriteAllBytes(filepath, loadDb.bytes);
		//}

		Constants.Dbcon = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

		Debug.Log("Db connection opened");

		Constants.MajorInfo = Constants.Dbcon.Table<MajorInfo>();

		Constants.StarInfo = Constants.Dbcon.Table<StarInfo>();

	}

	private static void CreateMajorObjects(GameObject planet, GameObject moon, GameObject star)
	{
		Debug.Log("CreateMajorObjects");


		IEnumerable<Major> majorObjects = Constants.Dbcon.Table<Major>();

		foreach (var majorObject in majorObjects) //TODO: handle dwarf and star
		{
			switch (majorObject.type)
			{
				case "planet":
					{
						if (DataPersistorScript.Instance.renderPlanets)
						{
							//Debug.Log("Creating object: " + majorObject.name);

							var position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(Input.location.lastData.longitude,
								Input.location.lastData.latitude, majorObject.name);

							float x, y, z;

							SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

							GameObject obj = Instantiate(planet, new Vector3(x, y, z), Quaternion.identity);

							obj.name = majorObject.name;
						}

						break;
					}
				case "moon":
					{
						//Debug.Log("Creating object: " + majorObject.name);
						if (DataPersistorScript.Instance.renderMoons)
						{
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

							obj.name = majorObject.name;
						}
						break;
					}
				case "dwarf planet":
					{
						//Debug.Log("Creating object: " + majorObject.name);
						if (DataPersistorScript.Instance.renderPlanets)
						{
							Vector3 position = new Vector3();
							if (!string.IsNullOrEmpty(majorObject.rot_pole_ra) && !string.IsNullOrEmpty(majorObject.rot_pole_de))
							{
								position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(Input.location.lastData.longitude,
									Input.location.lastData.latitude, majorObject.name);
							}

							float x, y, z;

							SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

							GameObject obj = Instantiate(moon, new Vector3(x, y, z), Quaternion.identity);

							obj.name = majorObject.name;
						}
						break;
					}
				case "star":
					{

						//Debug.Log("Creating object: " + majorObject.name);

						if (!string.IsNullOrEmpty(majorObject.rot_pole_ra) && !string.IsNullOrEmpty(majorObject.rot_pole_de) && DataPersistorScript.Instance.renderStars)
						{
							var position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(Input.location.lastData.longitude,
								Input.location.lastData.latitude, majorObject.name);
							float x, y, z;

							SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

							GameObject obj = Instantiate(star, new Vector3(x, y, z), Quaternion.identity);

							obj.name = majorObject.name;
						}




						break;
					}

			}
		}
	}

	private static void CreateStars(GameObject exemplaryStar)
	{
		Debug.Log("CreateStars");

		//var exemplaryStar = GameObject.FindWithTag("Star");

		IEnumerable<Stars> stars = Constants.Dbcon.Table<Stars>();

		foreach (var star in stars)
		{
			if (star.ra == 0 || star.dec == 0)
				continue;
			Vector3 position = CelestialCoordinates.CalculateHorizontalCoordinatesStar(Input.location.lastData.longitude,
				Input.location.lastData.latitude, (float)star.ra, (float)star.dec);
			float x, y, z;

			SphericalToCartesianFromPosition(out x, out y, out z, Constants.MeanHeading, position);

			var obj = Instantiate(exemplaryStar, new Vector3(x, y, z), Quaternion.identity);

			obj.name = star.proper;
			obj.SetActive(true);

		}

		exemplaryStar.SetActive(false);
	}

	private static void SphericalToCartesian(out float x, out float y, out float z)
	{
		const float radius = 20;
		var theta = Random.Range(0, 360) * Mathf.Deg2Rad;
		var phi = Random.Range(0, 180) * Mathf.Deg2Rad;

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

		float sumHeading = 0;

		while (Constants.MeanHeading <= 0)
		{
			for (var i = 0; i < 10000; i++)
			{
				sumHeading += Input.compass.trueHeading;
			}
			Constants.MeanHeading = sumHeading / 10000;
		}

		Debug.Log("meanHeading: " + Constants.MeanHeading);

		print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " +
			  Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " +
			  Input.location.lastData.timestamp);

		Debug.Log("Maj: " + DataPersistorScript.Instance.renderMajor + ", Min: " + DataPersistorScript.Instance.renderMinor + ", Moons: " + DataPersistorScript.Instance.renderMoons + ", Stars: " + DataPersistorScript.Instance.renderStars + ", Planets: " + DataPersistorScript.Instance.renderPlanets);
		GameObject planet = GameObject.FindWithTag("Planet");

		GameObject moon = GameObject.FindWithTag("Moon");

		GameObject star = GameObject.FindWithTag("Star");

		if (DataPersistorScript.Instance.renderMajor)
			CreateMajorObjects(planet, moon, star);
		if (DataPersistorScript.Instance.renderStars)
			CreateStars(star);



		planet.SetActive(false);
		star.SetActive(false);
		moon.SetActive(false);
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