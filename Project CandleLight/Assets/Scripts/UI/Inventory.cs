using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public Button IBPrefab;

    public List<Item> ItemInventory = new List<Item>();

    public List<Button> ItemButtons = new List<Button>();

    public static List<bool> itemToggles = new List<bool>();

    public static int CurrentGearCapacity = 0;

	// Use this for initialization
	void Start () {
        ItemInventory.Capacity = 12;
        itemToggles.Capacity = 12;
        ItemButtons.Capacity = 12;

        foreach (Item invitems in ItemInventory)
        { SpawnInvButton(invitems); }
        
	}
	

    public void SpawnInvButton(Item I)
    {
        if (ItemInventory.Count >= ItemInventory.Capacity) return;
        ItemInventory.Add(I);
        Button ItemCell = Instantiate(IBPrefab, transform);
        ItemCell.onClick.AddListener(() => ItemGet(I));
        ItemCell.gameObject.GetComponent<Image>().sprite = I.sprite;
    }


    //dont forget, if removing a button, be sure to RemoveListener as well from the button


    public void ItemGet(Item I)
    {
        Debug.Log("Setting Item: " + I.ItemName);
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
