using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFunctions : MonoBehaviour
{
    public static ItemFunctions ImFu;

    /*each item comes with an item code
      give the ItemFunctions a reference to the following

        player 

        Item Class

      so that ItemFunctions can have an easier time referencing what to do with what 

      */


    private void Awake()
    {
        if (ImFu == null)
        {
            DontDestroyOnLoad(gameObject);
            ImFu = this;
        }

        else if (ImFu != this)
            Destroy(gameObject);

    }

   

    public static void ItemFunction(string itemCode)
    {
        switch (itemCode)
        {
            default:
                Debug.Log("No Item");
                break;
        }
    }
}
