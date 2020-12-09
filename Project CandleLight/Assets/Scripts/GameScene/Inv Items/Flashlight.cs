using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{

    //try adding the values during initialization of the Flashlight object, perhaps a prefab.
    /// <summary>
    /// actually... can you just... 
    /// add A Flashlight prefab to the MiniInv and have Player call that and flip THAT active on and off?
    /// set the BatteryCalc functionality to the MiniInv
    /// </summary>

    new public void ItemClone(Item I)
    {
        if (I.ItemName != null)
            ItemName = I.ItemName;


        if (I.ItemType != null)
            ItemType = I.ItemType;

        if (I.sprite != null)
            sprite = I.sprite;

        if (I.description != null)
            description = I.description;

        if (I.code != null)
            code = I.code;

        SlotNumber = I.SlotNumber;

        if (I.SourceType != null)
            SourceType = I.SourceType;

        if (I.AreaString != null)
            AreaString = I.AreaString;
    } 

}
