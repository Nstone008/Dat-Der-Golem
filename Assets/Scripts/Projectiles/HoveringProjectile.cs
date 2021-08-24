using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringProjectile : Projectile
{
    public float distanceApart;

    public GameObject orb1;
    public GameObject orb2;
    public CircleCollider2D orb1Collider;
    public CircleCollider2D orb2Collider;
    public float turnSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        //transform.SetParent(sourceEntity.transform);
        var temp = GameObject.FindGameObjectWithTag("Player");
        transform.SetParent(temp.transform);
        transform.SetPositionAndRotation(transform.parent.position, Quaternion.identity);

        orb1.SetActive(true);
        orb2.SetActive(true);

        PlaceDistanceFromCharacter(distanceApart);

        Debug.Log(orb1.GetComponent<RectTransform>().anchoredPosition);
        Debug.Log(orb2.GetComponent<RectTransform>().anchoredPosition);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f,0f,turnSpeed);
    }

    void PlaceDistanceFromCharacter(float distance)
    {
        orb1.GetComponent<RectTransform>().anchoredPosition = new Vector2(distance, 0f);
        orb1Collider.offset = new Vector2(distance, 0f);
        orb2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-distance, 0f);
        orb2Collider.offset = new Vector2(-distance, 0f);

        orb1Collider.enabled = true;
        orb2Collider.enabled = true;
    }
}
