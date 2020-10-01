using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightSystem : Interactable
{

    public GameObject MasterLightSystem;

    public GameObject[] MinorLights;

    public Door[] DoorLights;

    public GameObject Player; 

    // Start is called before the first frame update
    void Start()
    {
        MasterLightSystem.gameObject.SetActive(false);
        this.InterType = "MasterLights";
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterLightSystem.gameObject.activeSelf)
        {
            Player.GetComponent<Movement>().DayTimeTrigger();
            for (int i = 0; i < DoorLights.Length; i++)
             DoorLights[i].DayTimeTrigger(); 
        }
        else
        {
            Player.GetComponent<Movement>().NightTimeTrigger();
            for (int i = 0; i < DoorLights.Length; i++)
             DoorLights[i].NightTimeTrigger(); 
        }
    }

    public void lightFlip()
    {
        MasterLightSystem.gameObject.SetActive(!MasterLightSystem.gameObject.activeSelf);

        for(int i = 0; i < MinorLights.Length; i++)
        {
            MinorLights[i].gameObject.SetActive(!MinorLights[i].gameObject.activeSelf);
        }

    }

}
