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
    //public Transform ItemPoint;                 //The space where our held object stays
    //public Transform DropPoint;                 //where we'd put down the object    
    RaycastHit2D ItemDetectRay;                 //check for Items to your front/center
    //RaycastHit2D LeftItemDetectRay;           //Check for Items to your left
    //RaycastHit2D CrouchItemDetectRay;         //Check for Items to your below


    private int facing = 1;                     //Check which way we're facing. 1 = right, -1 = left.  
    private bool Lit = false;

    float itemadditionalsize;
    float baseStandingColliderSizeX;

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


        HandleMovement();
        RunCheck();
        Grounded = (Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, GroundLayerMask) || Physics2D.OverlapCircle(GroundPoint.position, GroundPointCheckRadius, ItemsLayerMask));
        IsOnItem();

        if (!Grounded || vertical < 0) canJump = false;
        else canJump = true;
        //JumpState

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) Jump();
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Pickup")) PickUp();

        Lit = (Physics2D.OverlapCircle(transform.position, LightCheckRadius ,LightLayerMask));

        if (Lit) Debug.Log("Within Light, Edge Radius = " + myStandingCollider.edgeRadius);
        else Debug.Log("Within Darkness");

        /*if (Input.GetKeyDown(KeyCode.Backspace)||Input.GetButtonDown("Pause"))
        {
            Debug.Log("Paused");
            Menu.gameObject.SetActive(MenuToggle);
            MenuToggle = !MenuToggle;
        }*/
    }


    private void FixedUpdate()
    {
        

    }

    private void HandleMovement()
    {
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

        myRigidBody.velocity = new Vector2(horizontal * Speed, myRigidBody.velocity.y);
    }


    private void Jump()
    {
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

            return;
        }//ItemHold != null, that is, we're holding something
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
