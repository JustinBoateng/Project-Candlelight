using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private float xMax;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMin;
                              //these four variables will solidify the range of the camera's movement
    public Transform target; //This will allow the camera to concentrate on the player

	// Use this for initialization
	void Start () {

       //target = GameObject.Find("Teller").transform;
                               //Be sure that the name in the parenthesis is the name of the object you want the camera to follow
	}
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
                             //We're setting the transform position of the camera equal to the target's x position and the target's y position.
                             //But by using the "Clamp" method of Mathf, we make sure that the camera never goes below the min values of x or y, nor the max values of x or y
 	                         //Note the "Vector3" instead of the "Vector2". The third axis (z) needs to be accounted for. so we make sure the camera follows the player there too
    }
}
