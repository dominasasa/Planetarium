using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HandleOnClick : MonoBehaviour
{
	RaycastHit _hit;
	Ray _ray;

	IEnumerator FetchImage(string url, Image img)
	{
		using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("ERROR: FAILED TO FETCH");
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("NO ERROR");
				Texture2D t = ((DownloadHandlerTexture)www.downloadHandler).texture;
				img.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (!Physics.Raycast(_ray, out _hit)) return;
		Debug.Log(" you clicked on " + _hit.collider.gameObject.name);
		MenuManager.Instance.switchToMenu(5);

		var majorInfo = Constants.MajorInfo.Where(x => x.name == _hit.collider.gameObject.name);

		var starInfo = Constants.StarInfo.Where(x => x.proper == _hit.collider.gameObject.name);

		Image img = GameObject.Find("SkyObjImage").GetComponent<Image>();
		Text text = GameObject.Find("Print target notice").GetComponent<Text>();

		bool majorNull = false;
		bool starNull = false;

		foreach (var obj in majorInfo)
		{
			if (obj.name == null)
			{
				majorNull = true;
				continue;
			}
			text.text = obj.summary;
			StartCoroutine(FetchImage(obj.imgLink, img));
		}

		foreach (var obj in starInfo)
		{
			if (obj.name == null)
			{
				starNull = true;
				continue;
			}
			text.text = obj.summary;
			StartCoroutine(FetchImage(obj.imgLink, img));
		}

		if (!starNull || !majorNull)
			return;
		text.text = "Sorry, we do not have any information on this object";
		StartCoroutine(FetchImage("https://gibsonlane.co.uk/wp-content/uploads/2018/08/No-information-available-300x136.png", img));

	}
}
