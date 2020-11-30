﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniInv : MonoBehaviour
{
    public static MiniInv MI;

    public GameObject ItemADisplay;
    public GameObject ItemBDisplay;

    public Item ItemA;
    public Item ItemB;

    //public Inventory InvReference;

    // Start is called before the first frame update
    void Start()
    {
        MI = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if "A is pressed, run UseItemA"
        //Let Player have a reference to MiniInv
   
    }

    public void MiniInvSetA(Item A)
    {
        if(ItemB == A)
        {
            ItemB = null;
            ItemBDisplay.GetComponent<Image>().sprite = null;
        }
            ItemA = A;

        ItemADisplay.GetComponent<Image>().sprite = A.sprite;

    }

    public void MiniInvSetB(Item B)
    {
        if (ItemA == B)
        {
            ItemA = null;
            ItemADisplay.GetComponent<Image>().sprite = null;
        }

        ItemB = B;

        ItemBDisplay.GetComponent<Image>().sprite = B.sprite;

    }

    //The Player class will call the UseItem Functions, passing down any important info if necesasry

    public void UseItemA(GameObject A)
    {
        if (A.GetComponent<Door>()) 
           {
            if(ItemA.GetComponent<Keys>())
             ItemA.GetComponent<Keys>().ItemUse(A.GetComponent<Door>());

            else if (ItemA.GetComponent<KeyChain>())
             ItemA.GetComponent<KeyChain>().ItemUse(A.GetComponent<Door>());
        }
    }

    public void UseItemB(GameObject B)
    {
        if (B.GetComponent<Door>())
        {
            if (ItemB.GetComponent<Keys>())
                ItemB.GetComponent<Keys>().ItemUse(B.GetComponent<Door>());

            else if (ItemB.GetComponent<KeyChain>())
                ItemB.GetComponent<KeyChain>().ItemUse(B.GetComponent<Door>());
        }
    }
    //------------------------------------------------------------------------------------------------
}
