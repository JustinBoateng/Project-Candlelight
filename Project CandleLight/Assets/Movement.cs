using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour {


    private Rigidbody2D myRigidBody;
    private BoxCollider2D myStandingCollider;
    private CapsuleCollider2D myCrouchingCollider;
    private Vector2 velocity;


    [SerializeField] private LayerMask GroundLayerMask;
    [SerializeField] private LayerMask ItemsLayerMask; //NOTE THE NEW ITEMS LAYER MASK ADDITION



    //Walking and Running Values
    private float horizontal;                   //Walking
    private float StrictHorizontal;             //Running
    private float BaseSpeed = 5;                //Original Speed
    private float Speed = 5;                    //Current Speed
    private bool runState = false;              //Running State Check
    private float tapTime = 0;                  //DoubleTap inputCheck for Running
    private float lastTapTime;          
    private int tapCheck = 0;

    //Image Flipping Value
    private float originalxscale;       


    //Jumping and Falling Values
    private float vertical;                     //base Jumping or Falling Speed
    private float jumpVelocity = 8;             //fine-tuned jumping speed   
    public Transform GroundPoint;               //Grounded Check
    public float GroundPointCheckRadius;        //GroundPointCircle Radius to finetune if we're grounded or not
    private bool Grounded = false;              //Grounded State check
    private float fallMultiplier = 6.19f;       //Fine-tuned Falling
    private float lowJumpMultiplier = 5.85f;    //Short-Jump Value

    //Item Interaction Values
    private bool ItemCheck = false;             //Are we in front of an item
    private GameObject itemIdle;                //The name of the item we are in front of
    private GameObject itemHold;                //The name of the item we are holding
    private bool Holding = false;               //Are we Holding an Object right now?
    public Transform HoldPoint;                 //The space where our held object stays
    public Transform DropPoint;                 //where we'd put down the object    
    RaycastHit2D ItemDetectRay;                 //check for Items to your front/center
    //RaycastHit2D LeftItemDetectRay;           //Check for Items to your left
    //RaycastHit2D CrouchItemDetectRay;         //Check for Items to your below


    private int facing = 1;                     //Check which way we're facing. 1 = right, -1 = left.  



    // Use this for initialization
    void Start () {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myStandingCollider = gameObject.GetComponent<BoxCollider2D>();
        myCrouchingCollider = gameObject.GetComponent<CapsuleCollider2D>();
        velocity = myRigidBody.velocity;
        originalxscale = transform.localScale.x;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
