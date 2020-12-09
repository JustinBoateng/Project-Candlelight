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

    public string AreaString;
    //AreaString is used to reference which Area you got the item from. Useful for keys

    public Item() { }
    
    public Item(string i)
    {
        ItemName = "Empty";
        ItemType = "Empty";
        description = "Empty";
        code = "Empty";
        SourceType = "Empty";
        AreaString = "Empty";

    }
    //constructor, for the case of making new Items in the Inventory Tab 

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

        if (I.AreaString != null)
            AreaString = I.AreaString;
    }

    public void ItemUse()
    {
        Debug.Log("Using the Item Class");
    }

    //ItemClone is used to give a copy of the item to the Inventory Menu

}
