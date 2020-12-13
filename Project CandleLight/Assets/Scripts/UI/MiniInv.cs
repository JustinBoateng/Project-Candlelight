using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniInv : MonoBehaviour
{
    public Movement AttachedPlayer;

    public GameObject ItemADisplay;
    public GameObject ItemBDisplay;

    public Item ItemA;
    public Item ItemB;

    //public Inventory InvReference;
    public bool FlashlightEquipped;


    // Update is called once per frame
    void Update()
    {
        //if "A is pressed, run UseItemA"
        //Let Player have a reference to MiniInv

        if (ItemA)
        {
            if (ItemA.code == "Flashlight")
            {
                FlashlightEquipped = true;
            }
        }

        else if (ItemB)
        {
            if (ItemB.code == "Flashlight")
            {
                FlashlightEquipped = true;
            }
        }

        if (!FlashlightEquipped) AttachedPlayer.FlipOff("Battery");

    }

    public void MiniInvSetA(Item A)
    {
        if (ItemB == A)
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
        if (ItemA == null) return;

        if (A.GetComponent<Door>())
        {
            if (ItemA.GetComponent<Keys>())
                ItemA.GetComponent<Keys>().ItemUse(A.GetComponent<Door>());

            else if (ItemA.GetComponent<KeyChain>())
                ItemA.GetComponent<KeyChain>().ItemUse(A.GetComponent<Door>());
        }



    }

    public void UseItemB(GameObject B)
    {
        if (ItemB == null) return;

        if (B.GetComponent<Door>())
        {
            if (ItemB.GetComponent<Keys>())
                ItemB.GetComponent<Keys>().ItemUse(B.GetComponent<Door>());

            else if (ItemB.GetComponent<KeyChain>())
                ItemB.GetComponent<KeyChain>().ItemUse(B.GetComponent<Door>());
        }

    }


    public void UseItemASolo()
    {
        if (ItemA == null) return;

        if (ItemA.code == "Flashlight")
        {
        
                AttachedPlayer.FlipItemLight("Battery");

        }
        if (ItemA.code == "Lanturn")
        {

            AttachedPlayer.FlipItemLight("Oil");

        }
    }

    public void UseItemBSolo()
    {
        if (ItemB == null) return;

        if (ItemB.code == "Flashlight")
        {
            AttachedPlayer.FlipItemLight("Battery");
        }

        if (ItemB.code == "Lanturn")
        {
            AttachedPlayer.FlipItemLight("Oil");
        }
    }
    

    //------------------------------------------------------------------------------------------------
}
