using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyChain : Item
{
    public List<char> AvailableKeys;

    public void KeyChainClone(KeyChain I)
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

        if (I.AreaString != "")
            AreaString = I.AreaString;

        if (I.AvailableKeys != null)
            AvailableKeys = I.AvailableKeys;
    }
}
