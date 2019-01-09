using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HandleClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log("Name = " + hit.collider.name);
				Debug.Log("Tag = " + hit.collider.tag);
				Debug.Log("Hit Point = " + hit.point);
				Debug.Log("Object position = " + hit.collider.gameObject.transform.position);
				Debug.Log("--------------");
			}
		}
	}

	private void OnMouseDown()
	{
		Debug.Log(" you clicked on " + this.name);
		MenuManager.Instance.switchToMenu(5);

		var majorInfo = Constants.MajorInfo.Where(x => x.name == this.name);

		var starInfo = Constants.StarInfo.Where(x => x.proper == this.name);

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

	public IEnumerator FetchImage(string url, Image img)
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
}
