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


    public bool Locked = false;
    public Sprite[] LockIndicator = new Sprite[2]; //0 = locked //1 = not locked
    public SpriteRenderer LockLight;

    public bool DayTime = false;

    public char DoorLetter;
    //DoorLetter is supposed to reference the particular door with keys and the area.
    //You're going to need to give each Door its own door letter manually. 
    //More specifically, only those that are free to be locked/unlocked or doors not attached to puzzles

    public string AreaString;
    //AreaString is supposed to identify if the keys used on it are even of the same area.
    //a key can work on a door if the AreaString and DoorLetter match


    public void Start()
    {
        this.InterType = "Door";
       
    }

    public void Update()
    {
        // Tells the player if the door on the other side is within light-----------------------------------------
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

        else if (DayTime) withinLight = true;

        else withinLight = false;

        if(GetOtherDoor().isinLight())
            this.GetComponent<SpriteRenderer>().sprite = DoorVisual[1];
        else
            this.GetComponent<SpriteRenderer>().sprite = DoorVisual[0];


        //---------------------------------------------------------------------------------------------------------


        //Tells the player if the door is locked or not------------------------------------------------------------
        if (!Locked) LockLight.sprite = LockIndicator[1];
        else LockLight.sprite = LockIndicator[0];
        //---------------------------------------------------------------------------------------------------------

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

    public bool getLock()
    {
        return Locked;
    }

    public void flipLock()
    {
        Locked = !Locked;
    }

    public void setLock(bool Status)
    {
        Locked = Status;
    }

    public void DayTimeTrigger()
    {
        DayTime = true;
    }

    public void NightTimeTrigger()
    {
        DayTime = false;
    }
}
