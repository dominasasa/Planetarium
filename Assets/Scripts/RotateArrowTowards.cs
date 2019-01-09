using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateArrowTowards : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject target;

    
    // Update is called once per frame
    void Update()
    {
        Vector3 VPposition = Camera.current.WorldToScreenPoint(target.transform.position);
        Vector2 direction = new Vector2(VPposition.x - transform.position.x, VPposition.y - transform.position.y);

        if(Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2)) < 50)
        {
            gameObject.GetComponent<Image>().enabled = false;
        } else
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);

    }
}
