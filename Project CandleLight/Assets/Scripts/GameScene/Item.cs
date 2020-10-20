using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item  : MonoBehaviour {

    public string ItemName;
    public string ItemType;
    public Sprite sprite; //install a sprite renderer
    public string description;
    public string code;
    public int SlotNumber;
    public string SourceType = "";


    public void ItemClone(Item I)
    {
        if(I.ItemName != null)
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
    }

}
