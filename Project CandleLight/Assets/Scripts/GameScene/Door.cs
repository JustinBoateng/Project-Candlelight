using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] Door OtherSide;

    [SerializeField] private LayerMask LightLayerMask; //detect items in Light using the raycast/boxcollider

    [SerializeField] Sprite[] DoorVisual = new Sprite[3];

    public bool withinLight;

    public float LightDetectDistance = 1;
    //BoxCollider2D LightDetector;
    RaycastHit2D LightDetectRayOne;
    RaycastHit2D LightDetectRayTwo;
    RaycastHit2D LightDetectRayThree;
    bool PlayerLight;
    [SerializeField] private LayerMask PlayerLayer;

    public void Start()
    {
        this.InterType = "Door";
       
    }

    public void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(-0.5f, -0.5f, 0), transform.up, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0, -0.5f, 0), transform.up, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0.5f, -0.5f, 0), transform.up, Color.blue);
        LightDetectRayOne = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -0.5f, 0), transform.up, LightDetectDistance, LightLayerMask);
        LightDetectRayTwo = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), transform.up, LightDetectDistance ,LightLayerMask);
        LightDetectRayThree = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.5f, 0), transform.up, LightDetectDistance, LightLayerMask);
        PlayerLight = Physics2D.OverlapCircle(transform.position, LightDetectDistance * 2, PlayerLayer);

        if (LightDetectRayOne.collider != null || LightDetectRayTwo.collider != null || LightDetectRayThree.collider != null)
            withinLight = true;

        else if (PlayerLight) withinLight = true;

        else
            withinLight = false;

        if(GetOtherDoor().isinLight())
            this.GetComponent<SpriteRenderer>().sprite = DoorVisual[1];
        else
            this.GetComponent<SpriteRenderer>().sprite = DoorVisual[0];
    }


    public Door GetOtherDoor()
    {
        return OtherSide;
    }

    public bool isinLight()
    {
        return withinLight;
    }

    public bool SafeDoor()
    {
        if (OtherSide.isinLight()) return true;

        return false;
    }

}
