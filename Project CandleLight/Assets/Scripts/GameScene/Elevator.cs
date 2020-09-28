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

}
