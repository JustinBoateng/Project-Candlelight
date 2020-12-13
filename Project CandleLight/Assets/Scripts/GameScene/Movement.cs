using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour {

    public Inventory InventoryReference;
    private bool MenuToggle = false;

    private Rigidbody2D myRigidBody;
    public BoxCollider2D myStandingCollider;
    //public CapsuleCollider2D myCrouchingCollider;
    public BoxCollider2D myCrouchingCollider;
    
    private Vector2 velocity;


    [SerializeField] private LayerMask GroundLayerMask;
    [SerializeField] private LayerMask ItemsLayerMask; //NOTE THE NEW ITEMS LAYER MASK ADDITION
    [SerializeField] private LayerMask LightLayerMask;
    [SerializeField] private LayerMask InteractableLayerMask; 
    public float LightCheckRadius;

    //Walking and Running Values
    private float horizontal;                   //Walking
    private float StrictHorizontal;             //Running
    public float BaseSpeed = 2;                //Original Speed
    public float Speed = 2;                    //Current Speed
    public float CrouchingSpeed = 0.5f;
    public float RunSpeed = 5;
    private bool runState = false;              //Running State Check
    private float tapTime = 0;                  //DoubleTap inputCheck for Running
    private float lastTapTime;          
    private int tapCheck = 0;
    private int facing = 1;                     //Check which way we're facing. 1 = right, -1 = left.  

    //Image Flipping Value
    private float originalxscale;       


    //Jumping and Falling Values
    private float vertical;                     //base Jumping or Falling Speed
    public float jumpVelocity = 6;             //fine-tuned jumping speed   
    private bool canJump = true;
    public Transform GroundPoint;               //Grounded Check
    public float GroundPointCheckRadius;        //GroundPointCircle Radius to finetune if we're grounded or not
    public Transform CeilingPoint;              //Ceiling Check
    public float CeilingPointCheckRadius;       //CeilingPointCircle Radius to finetune if there's something above the player while they're crouched
    
    private bool CrouchState = false;
    private bool Grounded = false;              //Grounded State check
    private float fallMultiplier = 6.19f;       //Fine-tuned Falling
    private float lowJumpMultiplier = 5.85f;    //Short-Jump Value

    //Item Interaction Values
    private bool ItemCheck = false;             //Are we in front of an item
    private GameObject itemIdle;                //The name of the item we are in front of
    private GameObject itemHold;                //The name of the item we are holding
    private bool Holding = false;               //Are we Holding an Object right now?
    public Transform HoldPoint;
    RaycastHit2D ItemDetectRay;                 //check for Items to your front/center
    float itemadditionalsize;
    float baseStandingColliderSizeX;

    //Light Detection Values
    private bool Lit = false;
    [SerializeField] LayerMask RegularMask;
    [SerializeField] LayerMask HoldingAGlowingObjectMask;
    private bool Daytime = false;

    //Interaction Values
    RaycastHit2D InteractableDetectRay;
    private GameObject interactIdle;
    private bool interactCheck = false;
    private Interactable InterObjReference;
    bool onLadder = false;
    private bool walkthroughdoor = false;

    public MiniInv MI;
    public GameObject[] InvItemLightTriggers;
    public int[] Energyleft;
    public int[] MaxEnergyleft;
    public int[] EnergyRate;
    public string InvItmEnergyType;

    public bool InvItmLightFlip = false;

    // Use this for initialization
    void Start ()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myStandingCollider.enabled = true;
        myCrouchingCollider.enabled = false;
        velocity = myRigidBody.velocity;
        originalxscale = transform.localScale.x;

        baseStandingColliderSizeX = myStandingCollider.size.x;

        Energyleft = new int [InvItemLightTriggers.Length];
        MaxEnergyleft = new int[InvItemLightTriggers.Length];
        EnergyRate = new int[InvItemLightTriggers.Length];
        InvItmEnergyType = "Empty";
        for (int i = 0; i < Energyleft.Length; i++)
        {
            Energyleft[i] = 100;
            MaxEnergyleft[i] = 100;
            EnergyRate[i] = 0;
        }

        InvokeRepeating("EnergyCalc", 2.0f, 0.5f);

    }

  


    private void FixedUpdate()
    {
        if (Input.GetKey(GameManager.GM.left))
        {
            horizontal = -1;
            StrictHorizontal = -1;
        }
        else if
        (Input.GetKey(GameManager.GM.right))
        {
            horizontal = 1;
            StrictHorizontal = 1;
        }
        else
        {
            horizontal = 0;
            StrictHorizontal = 0;
        }

        if (Input.GetKey(GameManager.GM.up)) vertical = 1;
        else if (Input.GetKey(GameManager.GM.down)) vertical = -1;
        else
        {
            vertical = 0;
        }

        
    }
    //putting these functions in fixedupdate rather than in regular update allows them to activate ONLY when the game is unpaused (because physics time moves in fixed update rather than in regular update)


    void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
        //StrictHorizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        Debug.Log("Using Energy Type: " + InvItmEnergyType);


        if (Input.GetKeyDown(GameManager.GM.jump)) Jump();
        
        //Button Config Based Movement (Assuming you're using Keyboard)

        if (onLadder) HandleLadderMovement();
        else HandleMovement();

        RunCheck();
        Grounded = (Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, GroundLayerMask) || Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, ItemsLayerMask));
        IsOnItem();
        IsNextTointeractable();

        if (!Grounded || vertical < 0) canJump = false;
        else canJump = true;
        //JumpState
        
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("jumpKey")) Jump();
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Pickup")) PickUp();
        //if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Vertical") == 1) && interactCheck == true) Interact(interactIdle.gameObject.GetComponent<Interactable>().getType());
        //if (Input.GetKeyUp(KeyCode.UpArrow)) walkthroughdoor = false;

        
        if (vertical == 1) walkthroughdoor = false;

        Lit = (Physics2D.OverlapCircle(transform.position, LightCheckRadius ,LightLayerMask));

        if (itemHold != null)
        {
            if ((itemHold.GetComponentInChildren<CircleCollider2D>()) && (itemHold.GetComponentInChildren<CircleCollider2D>().isActiveAndEnabled))
                gameObject.layer = LayerMask.NameToLayer("PlayerWithLight");
            //holding ANY light source ...
            //This works only if the Lights are off
            //...we finna light a torch with a flashlight aren't we?

            if (itemHold.GetComponent<Torch>().LitState != "notLit")
                gameObject.layer = LayerMask.NameToLayer("PlayerWithLight");
            //holding a light source from a torch speciifcally
        }
        //if the item emit's a light source, change player's mask to "PlayerWithLight" so that doors detect her as a light source, as well as other things 
        //here, we're assuming that the player is holding an object that has a light source
        //the assumption is that all circlecolliders (colliders that are circular specifically) are light source areas

        if ((Lit) || (gameObject.layer == LayerMask.NameToLayer("PlayerWithLight"))) Debug.Log("Within Light, Edge Radius = " + myStandingCollider.edgeRadius);
        else if (Daytime) Debug.Log("Within Light because Master Light is on");
        else Debug.Log("Within Darkness");

        //Lit here represents that Protag is within a light source
        if (InvItmLightFlip) gameObject.layer = LayerMask.NameToLayer("PlayerWithLight");
        else if (!InvItmLightFlip) gameObject.layer = LayerMask.NameToLayer("Player");



        if (Input.GetKeyDown(GameManager.GM.up) && interactCheck == true)
            Interact(interactIdle.gameObject.GetComponent<Interactable>().getType());

        if (Input.GetButtonDown("ItemA") && !MenuNavigation.MN.isPaused())
        {
            if (interactCheck)
            {
                Debug.Log("Using Item A");
                MI.UseItemA(interactIdle.gameObject);
            }

            Debug.Log("Using Item B Solo");
            MI.UseItemASolo();
        }


        if (Input.GetButtonDown("ItemB") && !MenuNavigation.MN.isPaused())
        {
            if (interactCheck)
            {
                Debug.Log("Using Item B");
                MI.UseItemB(interactIdle.gameObject);
            }

            Debug.Log("Using Item B Solo");
            MI.UseItemBSolo();
        }
        

    }


    private void HandleMovement()
    {
        myRigidBody.gravityScale = 1;

        if (horizontal > 0)
        {
            facing = 1;
            transform.localScale = new Vector2(originalxscale, transform.localScale.y);
        }

        if (horizontal < 0)
        {
            facing = -1;
            transform.localScale = new Vector2(-originalxscale, transform.localScale.y);
        }

        if (vertical < 0)
        {
            Speed = CrouchingSpeed;
            myCrouchingCollider.enabled = true;
            myStandingCollider.enabled = false;
        }
        else
        {
            Speed = BaseSpeed;
            myCrouchingCollider.enabled = false;
            myStandingCollider.enabled = true;
        }

        if (runState == true) Speed = RunSpeed; //running
        else Speed = BaseSpeed; //walking
        
        //if()//next to a ladder and pressing up

        myRigidBody.velocity = new Vector2(horizontal * Speed, myRigidBody.velocity.y);
    }

    private void HandleLadderMovement()
    {
        myRigidBody.gravityScale = 0;
        myRigidBody.velocity = new Vector2(0, vertical * Speed);

    }

    private void Jump()
    {
        if (onLadder) onLadder = false;
        if (canJump) myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, +1 * jumpVelocity);
    }

    private void IsOnItem()
    {
        Debug.DrawRay(transform.position + new Vector3(0, -0.25f, 0), transform.right * facing, Color.red);
        ItemDetectRay = Physics2D.Raycast(transform.position + new Vector3(0, -0.05f, 0), transform.right * facing, 0.5f, ItemsLayerMask);
        //ObjectDetectRay = Physics2D.Raycast(transform.position, transform.right * facing, 0.5f, ItemsLayerMask);
        //itemDetectArea = Physics2D.OverlapCircle(transform.position, ItemDetectDistance * 2, ItemsLayerMask);

        if (ItemDetectRay.collider != null)
        {
            ItemCheck = true;
            itemIdle = ItemDetectRay.collider.gameObject;
        }
        else
        {
            ItemCheck = false;
            itemIdle = null;
        }
    }

    private void IsNextTointeractable()
    {
        Debug.DrawRay(transform.position + new Vector3(0, -0.05f, 0), transform.right * facing, Color.blue);
        InteractableDetectRay = Physics2D.Raycast(transform.position + new Vector3(0, -0.05f, 0), transform.right * facing, 0.25f, InteractableLayerMask);
        //ObjectDetectRay = Physics2D.Raycast(transform.position, transform.right * facing, 0.5f, ItemsLayerMask);


        if (InteractableDetectRay.collider != null)
        {
            interactCheck = true;
            interactIdle = InteractableDetectRay.collider.gameObject;
        }
        else
        {
            interactCheck = false;
            interactIdle = null;
        }
    }

    private void PickUp()
    {
        if (itemHold == null) //if we're not holding an object
        {
            Debug.Log("Pickup activated");
            if (ItemCheck == true) //if we're next to an object
            {                              
                if (itemIdle.gameObject.GetComponent<Item>().ItemType == "OBJ" && itemHold == null)
                {
                    itemHold = itemIdle; //our held item is now the item next to Teller
                    

                    myStandingCollider.size = new Vector2(myStandingCollider.size.x + itemHold.GetComponent<BoxCollider2D>().size.x , myStandingCollider.size.y);

                    itemHold.GetComponent<Rigidbody2D>().simulated = false; //remove physics of the object for the time being


                    itemHold.transform.position = HoldPoint.position; //affixiate the object to the HoldPoint position

                    itemHold.transform.parent = gameObject.transform; //attach the object to Protag

                    //itemIdle = null; //Teller is no longer next to an object

                    Speed = BaseSpeed / 2;

                    //if ((itemHold.GetComponentInChildren<CircleCollider2D>()) && (itemHold.GetComponentInChildren<CircleCollider2D>().isActiveAndEnabled)) gameObject.layer = LayerMask.NameToLayer("PlayerWithLight"); 
                    //if the item emit's a light source, change player's mask to "PlayerWithLight" so that doors detect her as a light source, as well as other things 
                    //here, we're assuming that the player is holding an object that has a light source
                    //the assumption is that all circlecolliders (colliders that are circular specifically) are light source areas

                    return;
                }//if it's a liftable item



                if (itemIdle.gameObject.GetComponent<Item>().ItemType == "INV")
                {
                    if (itemIdle.gameObject.GetComponent<Keys>())
                    {
                        if (InventoryReference.listOfKeyChains.Contains(itemIdle.gameObject.GetComponent<Keys>().AreaString))
                        {
                            InventoryReference.KeyPickUp(itemIdle.gameObject.GetComponent<Keys>());
                            itemIdle.gameObject.SetActive(false);
                            return;
                        }
                        //if a keychain already exists, JUST add the key to the keychain

                        InventoryReference.KeyPickUp(itemIdle.gameObject.GetComponent<Keys>());
                        InventoryReference.ItemInventory.Add(itemIdle.gameObject.GetComponent<Keys>());
                        InventoryReference.SpawnInvButton(itemIdle.gameObject.GetComponent<Keys>());
                        itemIdle.gameObject.SetActive(false);
                        return;
                        //else, add the key to your inventory itself
                    }
                    //A case SPECIFICALLY for if we're picking up keys
                    //if we already had a chain or key in inv, 
                    //then we can just add it to the collection by adding it to the keychain or creating a keychain

                    else
                    {
                        Debug.Log("INV Item Detected");
                        InventoryReference.ItemInventory.Add(itemIdle.gameObject.GetComponent<Item>());
                        InventoryReference.SpawnInvButton(itemIdle.gameObject.GetComponent<Item>());
                        itemIdle.gameObject.SetActive(false);
                        return;
                    }
                }//if it's an inventory item

            }
        }

        else
        {
            

            myStandingCollider.size = new Vector2(myStandingCollider.size.x - itemHold.GetComponent<BoxCollider2D>().size.x, myStandingCollider.size.y);

            itemHold.GetComponent<Rigidbody2D>().simulated = true;

            itemHold.transform.parent = null;

            itemHold.GetComponent<Rigidbody2D>().velocity = new Vector2((0.5f), 2);

            itemHold.transform.position = HoldPoint.position;

            Speed = BaseSpeed;

            itemHold = null;

            gameObject.layer = LayerMask.NameToLayer("Player"); //by dropping an object with light, the player is, in of themselves, no longer a light source

            return;
        }//ItemHold != null, that is, we're holding something
    }

    private void Interact(String InterType)
    {
        Debug.Log("Interacting with: " + InterType);

        switch (InterType)
        {
            case "Ladder":
                if (onLadder || itemHold != null) break; //if we're already on a ladder, or holding an object, then we dont START climbing the ladder again/at all
                onLadder = true; //set movement type to Ladder mode
                transform.position = new Vector3(interactIdle.gameObject.transform.position.x, transform.position.y, transform.position.z); //snap protag to ladder's x position      
                interactIdle.gameObject.GetComponent<Ladder>().PlatformToggle(); //toggle platforms attached to the ladder                
                break;

            case "Door":
                if (walkthroughdoor == true) break;//press up once, go through the door once. No holding to snap back and forth per frame                
                if (interactIdle.gameObject.GetComponent<Door>().getLock() == true) break; //if the door is locked (true), dont bother
                Door OtherDoor = interactIdle.gameObject.GetComponent<Door>().GetOtherDoor();//if the other door is within darkness... (to be continued)

                transform.position = new Vector3(OtherDoor.gameObject.transform.position.x, OtherDoor.gameObject.transform.position.y, -1); //we want to move to the same spot in 2d, but NOT in 3d. We want to keep protag in the same z-axis -1 value spot
                walkthroughdoor = true;
                //gameObject.transform.SetParent(GameObject.Find("PlayerObject").transform);
                break;

            case "Elevator":
                if (walkthroughdoor == true) break;//press up once, go through the door once. No holding to snap back and forth per frame 
                interactIdle.gameObject.GetComponent<EleSwitch>().FlipTrigger = true;
                walkthroughdoor = true;
                break;


            case "MasterLights":
                if (walkthroughdoor == true) break;
                interactIdle.gameObject.GetComponent<LightSystem>().lightFlip();
                walkthroughdoor = true;
                break;
        }
    }


    private void RunCheck()
    {
        if (StrictHorizontal == 0)
        {
            runState = false;
            tapCheck = 0;
        }

        if (StrictHorizontal != 0 && tapCheck == 0)
        {
            tapCheck = 1;
            lastTapTime = Time.time - tapTime;

            if ((lastTapTime < 0.2) && Grounded && (itemHold == null))
            runState = true;

            tapTime = Time.time;
        }
    }

    public void DayTimeTrigger()
    {
        Daytime = true;
    }

    public void NightTimeTrigger()
    {
        Daytime = false;
    }

    public void FlipItemLight(String IIET)
    {
        Debug.Log("Using Flashlight");
        if (InvItmLightFlip == false)
        {

            if (IIET == "Battery")
            {

                InvItmEnergyType = IIET;

                Debug.Log("Using Energy Type: " + InvItmEnergyType);


                GameObject Lgt = Instantiate(InvItemLightTriggers[0], transform);
                Lgt.gameObject.SetActive(true);
                Lgt.transform.position = HoldPoint.position + new Vector3(0.9f * facing, 0, 0); //keep in mind the player's facing position
                Debug.Log("Spawned a light source");

                EnergyRate[0] = 1;
            }

            else if (IIET == "Oil")
            {

                InvItmEnergyType = IIET;

                Debug.Log("Using Energy Type: " + InvItmEnergyType);


                GameObject Lgt = Instantiate(InvItemLightTriggers[1], transform);
                Lgt.gameObject.SetActive(true);
                Lgt.transform.position = HoldPoint.position + new Vector3(0.1f * facing, 0, 0); //keep in mind the player's facing position
                Debug.Log("Spawned a light source");

                EnergyRate[1] = 1;
            }

            InvItmLightFlip = true;
        }

        
        else if (InvItmLightFlip == true)
        {
            if (InvItmEnergyType != "" && InvItmEnergyType != IIET)
            {
                //if the light is already on and you want to click-on another inv lightsource
                FlipOff(InvItmEnergyType);
                //turn off the current light source 
                //before putting on another
                FlipItemLight(IIET);
                //turn on the chosen light
                return;
                //exit the function
            }

            if (IIET == "Battery")
            {
                InvItmEnergyType = "";
                Destroy(GameObject.Find(InvItemLightTriggers[0].name + "(Clone)"));
                EnergyRate[0] = 0;
            }

            else if (IIET == "Oil")
            {
                InvItmEnergyType = "";
                Destroy(GameObject.Find(InvItemLightTriggers[1].name + "(Clone)"));
                EnergyRate[1] = 0;
            }

            InvItmLightFlip = false;
        }

    }

    public void FlipOff(string IIET)
    {
        if (IIET == "Battery")
        {
            Destroy(GameObject.Find(InvItemLightTriggers[0].name + "(Clone)"));
            EnergyRate[0] = 0;
        }

        else if (IIET == "Oil")
        {
            Destroy(GameObject.Find(InvItemLightTriggers[1].name + "(Clone)"));
            EnergyRate[1] = 0;
        }

        InvItmLightFlip = false;
    }

    public void EnergyCalc()
    {
        if (InvItmEnergyType == "Battery")
        {
            if (Energyleft[0] == 0) return;
            Energyleft[0] = Energyleft[0] - EnergyRate[0];

            return;
        }

        if (InvItmEnergyType == "Oil")
        {
            if (Energyleft[1] == 0) return;
            Energyleft[1] = Energyleft[1] - EnergyRate[1];

            return;
        }

        else if (InvItmEnergyType == "") gameObject.layer = LayerMask.NameToLayer("Player");
    }

}
