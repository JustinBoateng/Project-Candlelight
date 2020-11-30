using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
    Attach this class to the Inventory Tab
    This class aims to manage Inventory, Serving as the medium between 

        The player                         The inventory of the player's items
        picking up items       AND                     Updating with new items 
        in the game world                                    In the pause Menu

    Picking up an item in the Game World spawns a new button in the pause menu's inventory tab.
    
*/

public class Inventory : MonoBehaviour {

    public Button IBPrefab;
    public Button KeyButtonPrefab;
    public Button KeyChainButtonPrefab;

    public List<Item> ItemInventory = new List<Item>();

    public List<Button> ItemButtons = new List<Button>();

    public GameObject[] ItemButts;// = new GameObject[12];
    int ItemButtsNavigation = 0;
    

    public static List<bool> itemToggles = new List<bool>();

    public int GearCapacity = 12;

    [SerializeField]
    public Text DescriptionSpace;

    public MiniInv MIReference;

    Item ItemInInv;

    /*
     using the MIRef
     if a player selects one of the items
     Run ItemGet() to send that item's info to the MiniInv

        Like, MiniInv.SetItemA(Selecteditem)

        Instead of Taking in a "Button" click
        Instead run the function if

        Input.getkey (GameManager.ItemAUse) 
        
        -The ItemA Button is pressed while- 
        
        EventSystem.currentSelectedGameObject.GetComponent<Item>() 
        
        -(the selected object has an Item class, thus making it identifiable as an Item object in the Inventory)-

        At that point, MIReference.SetItemA should assign the Item's data to it's A-Side panel and item variable

       
     */


    public Sprite KeyChainSprite;

    public bool hasKeyChain;
    public List<string> listOfKeyChains;
    //we have to attach this sprite to the Inventory since the KeyChain is one of the only items spawned in the Inventory and not on the field


	// Use this for initialization
	void Start () {
        ItemInventory.Capacity = GearCapacity;
        itemToggles.Capacity = GearCapacity;
        ItemButtons.Capacity = GearCapacity;
        ItemButts = new GameObject[GearCapacity];
        //foreach (Item invitems in ItemInventory)
        //{ SpawnInvButton(invitems); }
        //for(int i = 0; i < ItemButtons.Capacity; i++)
        //{
        //    ItemButts[i] = new GameObject();
        //}
	}

    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Item>() && Input.GetButtonDown("ItemA"))
            ItemSetA(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Item>());

        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Item>() && Input.GetButtonDown("ItemB"))
            ItemSetB(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Item>());

    }

    public void ItemFill()
    {
        foreach (Item invitems in ItemInventory)
        { SpawnInvButton(invitems); }
    }

    
    public void ItemMenuClear()
    {
        ItemButtons.ForEach(ItemClear);
    }
    
    public void ItemClear(Button I)
    {
        I.onClick.RemoveAllListeners();
        Destroy(I);
    }
    

    public void SpawnInvButton(Item I)
    {
        if (ItemInventory.Count >= ItemInventory.Capacity) return;

        if (I.GetComponent<Keys>())
        {
            Button KeyCell = Instantiate(KeyButtonPrefab, transform); //create the KeyButton 
            KeyCell.GetComponent<Keys>().KeyClone(I.GetComponent<Keys>()); //import the information to that button
            KeyCell.gameObject.GetComponent<Image>().sprite = I.sprite;    //set the KeyCell's image
            ItemButtons.Add(KeyCell);                                      //Add the button to the ItemButtons list
            ItemButts[ItemButtsNavigation] = KeyCell.gameObject;
            ItemButtsNavigation++;
            return;
        }

        else if (I.GetComponent<KeyChain>())
        {
            Button KeyChainCell = Instantiate(KeyChainButtonPrefab, transform); //create the KeyButton 
            KeyChainCell.GetComponent<KeyChain>().KeyChainClone(I.GetComponent<KeyChain>()); //import the information to that button
            KeyChainCell.gameObject.GetComponent<Image>().sprite = I.sprite;    //set the KeyCell's image
            ItemButtons.Add(KeyChainCell);                                      //Add the button to the ItemButtons list
            ItemButts[ItemButtsNavigation] = KeyChainCell.gameObject;
            ItemButtsNavigation++;
            return;
        }

        else
        {
            Button ItemCell = Instantiate(IBPrefab, transform);
            ItemCell.GetComponent<Item>().ItemClone(I.GetComponent<Item>());
            ItemCell.gameObject.GetComponent<Image>().sprite = I.sprite;
            ItemButtons.Add(ItemCell);
            ItemButts[ItemButtsNavigation] = ItemCell.gameObject;
            ItemButtsNavigation++;
            //ItemCell.onClick.AddListener(() => ItemSetA(I));
        }
    }
    

    //dont forget, if removing a button, be sure to RemoveListener as well from the button


    public void ItemSetA(Item I)
    {
        Debug.Log("Setting Item: " + I.ItemName);

        MIReference.MiniInvSetA(I);
    }


    public void ItemSetB(Item I)
    {
        Debug.Log("Setting Item: " + I.ItemName);

        MIReference.MiniInvSetB(I);
    }

    public void KeyPickUp(Keys K)
    {
        bool haskeys = false;
        bool hasChain = false;
        
        Item[] InvRef = ItemInventory.ToArray();
        Button[] ButtRef = ItemButtons.ToArray();
        int ButtonRef = 0;
        int spottoremove = 0;

        //we aim to use an array to go through the list and not just the list itself because we may need to go through multiple keys and check multiple areastrings 

        Debug.Log("ItemInventory Size is " +  ItemInventory.Count);

        for (int i = 0; i < ItemInventory.Count; i++)
        {
            ItemInInv = InvRef[i].GetComponent<Item>();
            ButtonRef = i;

            Debug.Log("Checking if we can make a keychain");
            Debug.Log("InvItem's AreaString is" + ItemInInv.GetComponent<Item>().AreaString);
            Debug.Log("New Key's AreaString is" + K.AreaString);

            if (InvRef[i].GetComponent<KeyChain>())
            {
                
                if (InvRef[i].GetComponent<KeyChain>().AreaString == K.AreaString)
                {
                    hasChain = true;
                    //spottoremove = i;
                    break;
                }
            }

            else if (InvRef[i].GetComponent<Keys>())
            {
                Debug.Log("Keychain can be made");

                if (InvRef[i].GetComponent<Keys>().AreaString == K.AreaString)
                    {
                        haskeys = true;
                        spottoremove = i;
                        break;
                    }
            }
            //for each scenario we must make sure to check if it's a keychain or a key, and then it's AreaString
        }
        if (hasChain)
        {
            Debug.Log("Adding it to the chain");

            ButtRef[ButtonRef].GetComponent<KeyChain>().AvailableKeys.Add(K.KeyLetter);
            ButtRef[ButtonRef].GetComponent<KeyChain>().description += ", " + K.KeyLetter;

        }

        else if (haskeys)
        {
            Debug.Log("Creating a keychain");


            GameObject NewKeyChain = new GameObject();
            //NewKeyChain.AddComponent<Item>();
            NewKeyChain.AddComponent<KeyChain>();

            NewKeyChain.GetComponent<KeyChain>().sprite = KeyChainSprite;
            NewKeyChain.GetComponent<KeyChain>().AreaString = K.AreaString;
            NewKeyChain.GetComponent<KeyChain>().description = "A batch of keys for doors in " + NewKeyChain.GetComponent<KeyChain>().AreaString + " for doors " + ItemInInv.GetComponent<Keys>().KeyLetter + ", " + K.KeyLetter;
            NewKeyChain.GetComponent<KeyChain>().AvailableKeys = new List<char>();
            NewKeyChain.GetComponent<KeyChain>().AvailableKeys.Capacity = 10;
            NewKeyChain.GetComponent<KeyChain>().AvailableKeys.Add(K.KeyLetter);
            NewKeyChain.GetComponent<KeyChain>().AvailableKeys.Add(ItemInInv.GetComponent<Keys>().KeyLetter);
            //Build the NewKeyChainItem

            ItemInventory.Add(NewKeyChain.GetComponent<KeyChain>());
            SpawnInvButton(NewKeyChain.GetComponent<KeyChain>());



            ItemInventory.RemoveAt(spottoremove);
            ButtRef[spottoremove].onClick.RemoveAllListeners();
            Destroy(ButtRef[spottoremove]);
            ItemButtons.RemoveAt(spottoremove);
            ButtRef[spottoremove].gameObject.SetActive(false);
            //Erase the previous Key to apply the keychain to the inv instead

            ItemButts[spottoremove] = null;
            ItemButtsNavigation--;
            //remove the key already in the inventory
            //since it'll be a part of the keychain now


            //spawn the NewKeyChain Item
            //for future reference, check to see if ItemButts is actually necessary

        }

        else
        {           
            listOfKeyChains.Add(K.AreaString);
            Debug.Log("New Key Added");
        }

    }
}
