using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public EleSwitch Switches; //three different colliders on the same single object 
  
    public Transform Apos;
    public Transform Bpos;
    public Transform nextPos;
    public float moveSpeed;

    public GameObject DoorA;
    public GameObject DoorB;
    public bool[] DoorStatuses = new bool[4];
    //[posAleftdoor | posArightdoor | posBleftdoor | posBrightdoor]
    //IF true, doors close, if false doors open
    public bool ElevatorMoving = false;


    public BoxCollider2D ElevatorArea;

    //be sure to attach an Interactable Script to the Elevator object with the Type "Elevator"
    private void Start()
    {
        Apos.position = transform.position;
        nextPos.position = Bpos.position;
        DoorOpen();
    }

    void Update () {

        if (Switches.FlipTrigger == true)
        {
            ElevatorTrigger();
            Switches.FlipTrigger = false;
        }

        if (ElevatorMoving) transform.position = Vector2.MoveTowards(transform.position, nextPos.position, moveSpeed* Time.deltaTime);

        if (transform.position == nextPos.position)
        {
            DoorOpen();
        }
    }

    public void ElevatorTrigger()
    {
        
        if (transform.position == Apos.position)
        {            
            nextPos.position = Bpos.position;
        }
        
        else if (transform.position == Bpos.position)
            nextPos.position = Apos.position;

        DoorClose();


        //set the course
        //THEN close doors and start the moving of the elevator
    }

    void DoorClose()
    {
        DoorA.SetActive(true);
        DoorB.SetActive(true);
        ElevatorMoving = true;
        
    }

    void DoorOpen()
    {
        if (transform.position == Apos.position)
        {
            nextPos.position = Bpos.position;

            if (!DoorStatuses[0]) DoorA.SetActive(false);
            else DoorA.SetActive(true);
            //set door status to that of DoorStatuses[0] and DoorStatuses[1]
            if (!DoorStatuses[1]) DoorB.SetActive(false);
            else DoorB.SetActive(true);

            ElevatorMoving = false;

            return;
        }

        else if (transform.position == Bpos.position)
        {
            nextPos.position = Apos.position;

            if (!DoorStatuses[2]) DoorA.SetActive(false);
            else DoorA.SetActive(true);
            //set door status to that of DoorStatuses[0] and DoorStatuses[1]
            if (!DoorStatuses[3]) DoorB.SetActive(false);
            else DoorB.SetActive(true);

            ElevatorMoving = false;

            return;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //float x = collision.gameObject.transform.localScale.x;
        //float y = collision.gameObject.transform.localScale.y;
        //float z = collision.gameObject.transform.localScale.z;
        //var originalScale = collision.gameObject.transform.localScale;
        //var originalxpos = collision.gameObject.transform.position.x;
        //var originalypos = collision.gameObject.transform.position.y;
        //var originalzpos = collision.gameObject.transform.position.z;

        //collision.gameObject.transform.SetParent(gameObject.transform, true);
        //collision.gameObject.transform.localScale = originalScale;
        /*collision.gameObject.transform.position = new Vector3(
            originalxpos,
            originalypos,
            originalzpos
            );
        if (ElevatorMoving && collision.gameObject.GetComponent<Movement>())
        {
            //collision.gameObject.transform.position = this.transform.position;
            collision.gameObject.transform.SetParent(gameObject.transform);
            collision.gameObject.transform.localScale = originalScale;
            //collision.gameObject.transform.localScale = new Vector3(0.69f, 0.69f, 1);
                x / this.GetComponentInParent<Transform>().localScale.x,
                y / this.GetComponentInParent<Transform>().localScale.y,
                z / this.GetComponentInParent<Transform>().localScale.z
                ); 
        }*/
        
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(this.transform);
            //collision.gameObject.transform.localScale = originalScale;
            //collision.gameObject.transform.localScale = new Vector3(x,y,z);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //var originalScale = collision.gameObject.transform.localScale;
        //collision.transform.SetParent(null, true);
        //collision.gameObject.transform.position = this.transform.position;

        collision.transform.SetParent(GameObject.Find("PlayerObject").transform);
    }
}
