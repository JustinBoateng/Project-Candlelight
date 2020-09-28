using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleSwitch : Interactable
{
    public bool FlipTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        this.InterType = "Elevator";
    }

}
