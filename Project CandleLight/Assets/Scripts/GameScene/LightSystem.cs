using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;


public class LightSystem : Interactable
{

    public GameObject MasterLightSystem;

    public GameObject[] MinorLights;
    public bool[] MinorLightsState;
    public GameObject[] StaticLights;
    //static lights are lights that the player can't change, like lamps or ceiling lights


    public Door[] Doors; //list of doors
    public Item[] ItemList;//List of items
    public string AreaTag;//area identification

    //at start, apply the AreaTags to each Doors' and items' AreaStrings

    public GameObject Player;

    public bool gameStart = false;

    public static LightSystem LS;

    private void Awake()
    {
        LS = this;
    }


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


        //for (int i = 0; i < MinorLightsState.Length; i++)
        //{
        //    MinorLightsState[i] = MinorLights[i].activeSelf;
        //}

    }

    // Update is called once per frame
    void Update()
    {

        if(gameStart == false)
        {
            MinorLightsStateCheck();
            gameStart = true;
        }
        //Pseudo Start Function to get the LightStates running

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

        //for (int i = 0; i < MinorLightsState.Length; i++)
        //{
        //   MinorLightsState[i] = MinorLights[i].activeSelf;
        //}
    }

    public void lightFlip()
    {


        if(!MasterLightSystem.gameObject.activeSelf)
            MinorLightsStateCheck(); 
        //store the light state values of each light source one last time 
        //if the MASTER LIGHTS WERE OFF BEFORE flipping the switch

        MasterLightSystem.gameObject.SetActive(!MasterLightSystem.gameObject.activeSelf);
        //flip the switch

        if (MasterLightSystem.gameObject.activeSelf)
        {           
            for (int i = 0; i < MinorLights.Length; i++)
            {
                MinorLights[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < StaticLights.Length; i++)
            {
                StaticLights[i].gameObject.SetActive(false);
            }

        }
        //if the Master light is on, then turn each minor light off

        else if (!MasterLightSystem.gameObject.activeSelf)
        {
            for (int i = 0; i < MinorLights.Length; i++)
            {
                MinorLights[i].gameObject.SetActive(MinorLightsState[i]);
            }

            for (int i = 0; i < StaticLights.Length; i++)
            {
                StaticLights[i].gameObject.SetActive(true);
            }

        }
        //if the Master light is off, then turn each minor light back to normal
    }

    public void MinorLightsStateCheck()
    {
        for (int i = 0; i < MinorLightsState.Length; i++)
        {
            MinorLightsState[i] = MinorLights[i].activeSelf;
        }
    }

}
