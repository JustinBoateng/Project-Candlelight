using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniInv : MonoBehaviour
{

    public GameObject ItemADisplay;
    public GameObject ItemBDisplay;

    public Item ItemA;
    public Item ItemB;

    //public Inventory InvReference;

    // Start is called before the first frame update
    void Start()
    {
        
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

    /*
     public void UseItemA()
     {
        ItemFunction(ItemA);
     }

     */

}
