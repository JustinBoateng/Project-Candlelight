using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : Item
{
    public char KeyLetter;

    //you're going to have to apply each key it's own individual letter manually

    public void KeyClone(Keys I)
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

        if (!I.KeyLetter.Equals(""))
            KeyLetter = I.KeyLetter;

        if (I.AreaString != null)
            AreaString = I.AreaString;
    }
    //we copy the info from the given item into the item button

    public void ItemUse(Door D)
    {
        Debug.Log("Using the Keys Class");
        Debug.Log("KeyArea: " + AreaString + ", DoorArea: " + D.AreaString);
        Debug.Log("Key: " + KeyLetter + ", Door: " + D.DoorLetter);

        if (AreaString == D.AreaString)
        {
            Debug.Log("Area: " + AreaString + ", Door: " + D.AreaString);
            if (KeyLetter.Equals(D.DoorLetter))
            {
                D.flipLock();
                D.GetOtherDoor().flipLock();
            }
        }
    }
}
