using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;
using Mono.Data.Sqlite;


public class PositionObjects : MonoBehaviour
{


	void Awake()
	{
		//LoadSQLite();

		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
	
		}

		// Access granted and location value could be retrieved
		print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);


		Vector3 position = CelestialCoordinates.CalculateHorizontalCoordinatesPlanets(20.950195, 52.196150, "Mercury");
		
		GameObject mercury = GameObject.FindGameObjectWithTag("Mercury");

		float radius = 100;
		float theta = position.y * Mathf.Deg2Rad;
		float phi = position.x * Mathf.Deg2Rad;

		float x = radius * Mathf.Cos(phi) * Mathf.Sin(theta);
		float y = radius * Mathf.Sin(phi);
		float z = radius * Mathf.Cos(theta) * Mathf.Cos(phi);

		mercury.transform.position = new Vector3(x, y, z);

		mercury.transform.rotation = Quaternion.AngleAxis(Input.compass.trueHeading,Vector3.right);

		CreateExamplaryStars();

		Input.location.Stop();


		//CreateExamplaryStars();
	}

	//private void LoadSQLite()
	//{
	//	// check if file exists in Application.persistentDataPath

	//	string filepath = Application.persistentDataPath + "/" + ;

	//	if (!File.Exists(filepath))

	//	{

	//		// if it doesn't ->

	//		// open StreamingAssets directory and load the db ->

	//		WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);  // this is the path to your StreamingAssets in android

	//		while (!loadDB.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

	//		// then save to Application.persistentDataPath

	//		File.WriteAllBytes(filepath, loadDB.bytes);

	//	}



	//	//open db connection

	//	var connection = "URI=file:" + filepath;

	//	var dbcon = new SqliteConnection(connection);

	//	dbcon.Open();
	//}

	private static void CreateExamplaryStars()
	{
		GameObject star = GameObject.FindWithTag("Star");

		float radius = 20;
		float theta = Random.Range(0, 360) * Mathf.Deg2Rad;
		float phi = Random.Range(0, 180) * Mathf.Deg2Rad;

		float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
		float y = radius * Mathf.Cos(phi);
		float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);


		star.transform.position = new Vector3(x, y, z);
		star.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);


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
	IEnumerator Start()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		// Start service before querying location
		//Input.location.Start();

		//// Wait until service initializes
		//int maxWait = 20;
		//while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		//{
		//	yield return new WaitForSeconds(1);
		//	maxWait--;
		//}

		//// Service didn't initialize in 20 seconds
		//if (maxWait < 1)
		//{
		//	print("Timed out");
		//	yield break;
		//}

		//// Connection has failed
		//if (Input.location.status == LocationServiceStatus.Failed)
		//{
		//	print("Unable to determine device location");
		//	yield break;
		//}

		//// Access granted and location value could be retrieved
		//print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

		//Vector2 position = CelestialCoordinates.CalculateHorizontalCoordinatesStar(Input.location.lastData.longitude,
		//	Input.location.lastData.latitude, 281.0097f, 61.4143f);

		//GameObject mercury = GameObject.FindGameObjectWithTag("Mercury");

		//float radius = 20;
		//float theta = position.y * Mathf.Deg2Rad;
		//float phi = position.x * Mathf.Deg2Rad;

		//float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
		//float y = radius * Mathf.Cos(phi);
		//float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

		//mercury.transform.position = new Vector3(x, y, z);

		//CreateExamplaryStars();

		//Input.location.Stop();

	}

	// Update is called once per frame
	void Update()
	{

	}
}
