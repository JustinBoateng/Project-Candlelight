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

    //Interaction Values
    RaycastHit2D InteractableDetectRay;
    private GameObject interactIdle;
    private bool interactCheck = false;
    private Interactable InterObjReference;
    bool onLadder = false;
    private bool walkthroughdoor = false;

    // Use this for initialization
    void Start () {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myStandingCollider.enabled = true;
        myCrouchingCollider.enabled = false;
        velocity = myRigidBody.velocity;
        originalxscale = transform.localScale.x;

        baseStandingColliderSizeX = myStandingCollider.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        StrictHorizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        if (onLadder) HandleLadderMovement();
        else HandleMovement();
        RunCheck();
        Grounded = (Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, GroundLayerMask) || Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, ItemsLayerMask));
        IsOnItem();
        IsNextTointeractable();

        if (!Grounded || vertical < 0) canJump = false;
        else canJump = true;
        //JumpState

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) Jump();
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Pickup")) PickUp();
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Vertical") == 1) && interactCheck == true) Interact(interactIdle.gameObject.GetComponent<Interactable>().getType());
        if (Input.GetKeyUp(KeyCode.UpArrow)) walkthroughdoor = false;

        Lit = (Physics2D.OverlapCircle(transform.position, LightCheckRadius ,LightLayerMask));

        if ((Lit) || (gameObject.layer == LayerMask.NameToLayer("PlayerWithLight"))) Debug.Log("Within Light, Edge Radius = " + myStandingCollider.edgeRadius);
        else Debug.Log("Within Darkness");

    }


    private void FixedUpdate()
    {
        

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
        ItemDetectRay = Physics2D.Raycast(transform.position + new Vector3(0, -0.25f, 0), transform.right * facing, 0.5f, ItemsLayerMask);
        //ObjectDetectRay = Physics2D.Raycast(transform.position, transform.right * facing, 0.5f, ItemsLayerMask);


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

                    if ((itemHold.GetComponentInChildren<CircleCollider2D>()) && (itemHold.GetComponentInChildren<CircleCollider2D>().isActiveAndEnabled)) gameObject.layer = LayerMask.NameToLayer("PlayerWithLight"); //if the item emit's a light source, change player's mask to "PlayerWithLight" so that doors detect her as a light source, as well as other things 

                    return;
                }//if it's a liftable item



                if (itemIdle.gameObject.GetComponent<Item>().ItemType == "INV")
                {
                    Debug.Log("INV Item Detected");
                    InventoryReference.SpawnInvButton(itemIdle.gameObject.GetComponent<Item>());
                    Destroy(itemIdle);
                    return;
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
                if (walkthroughdoor == true) break;//press up once, go through the door once. No holding to smap back and forth per frame                
                Door OtherDoor = interactIdle.gameObject.GetComponent<Door>().GetOtherDoor();
                transform.position = OtherDoor.gameObject.transform.position;
                walkthroughdoor = true;                
                break;

            case "Elevator":
                if (walkthroughdoor == true) break;//press up once, go through the door once. No holding to smap back and forth per frame 
                interactIdle.gameObject.GetComponent<EleSwitch>().FlipTrigger = true;
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

     
}
