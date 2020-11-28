using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightSystem : Interactable
{

    public GameObject MasterLightSystem;

    public GameObject[] MinorLights;
    public bool[] MinorLightsState;

    public Door[] Doors; //list of doors
    public Item[] ItemList;//List of items
    public string AreaTag;//area identification

    //at start, apply the AreaTags to each Doors' and items' AreaStrings

    public GameObject Player; 

    // Start is called before the first frame update
    void Start()
    {
        MasterLightSystem.gameObject.SetActive(false);
        this.InterType = "MasterLights";

        MinorLightsState = new bool[MinorLights.Length];

        for(int i = 0; i < Doors.Length; i++)
        {
            Doors[i].AreaString = AreaTag;
        }

        for (int i = 0; i < ItemList.Length; i++)
        {
            ItemList[i].AreaString = AreaTag;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (MasterLightSystem.gameObject.activeSelf)
        {
            Player.GetComponent<Movement>().DayTimeTrigger();
            for (int i = 0; i < Doors.Length; i++)
             Doors[i].DayTimeTrigger(); 
        }
        else
        {
            Player.GetComponent<Movement>().NightTimeTrigger();
            for (int i = 0; i < Doors.Length; i++)
             Doors[i].NightTimeTrigger(); 
        }
    }

    public void lightFlip()
    {

        for (int i = 0; i < MinorLights.Length; i++)
        {
            
                MinorLightsState[i] = MinorLights[i];

        }

        MasterLightSystem.gameObject.SetActive(!MasterLightSystem.gameObject.activeSelf);

        for(int i = 0; i < MinorLights.Length; i++)
        {
            if(MasterLightSystem.gameObject.activeSelf)
            MinorLights[i].gameObject.SetActive(false);

            if (!MasterLightSystem.gameObject.activeSelf)
                MinorLights[i].gameObject.SetActive(MinorLightsState[i]);
        }

    }

}
