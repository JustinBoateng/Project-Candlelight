using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public bool LitState = true;
    public LightTrigger Illuminescence;
    public SpriteRenderer FlameImage;

    private void Update()
    {
        Illuminescence.gameObject.SetActive(LitState);
        FlameImage.gameObject.SetActive(LitState); 
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            if (collider.gameObject.GetComponent<Item>().SourceType.ToUpper() == "FIRE")
            {
                if(collider.gameObject.GetComponent<Torch>().LitState == "isLit"){
                    LitState = true;
                }

                Debug.Log("Torch Detects Wood");
                collider.gameObject.GetComponent<Torch>().LitState = "isInFire";
            }
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerWithLight")|| collider.gameObject.layer == LayerMask.NameToLayer("Player")) //both layers since they can change for the player
        {
            Debug.Log("Detected Player");

            if (collider.gameObject.GetComponentInChildren<Item>())//check to see if the player is carrying something in the first place
            {
                Debug.Log("Detected Player With Item");
                if (collider.gameObject.GetComponentInChildren<Item>().SourceType.ToUpper() == "FIRE") //check to see the type of item it aludes to
                {
                    Debug.Log("Detected Player holding FIRE item");

                    if (collider.gameObject.GetComponentInChildren<Torch>().LitState == "isLit") //check the item for a Torch Script
                    {
                        Debug.Log("Detected Player Holding Fire Item that's Lit");

                        LitState = true;
                    }
                    Debug.Log("Torch Detects Wood in Child");
                    collider.gameObject.GetComponentInChildren<Torch>().LitState = "isInFire";
                }
            }
        }
    }


    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            if (collider.gameObject.GetComponent<Item>().SourceType.ToUpper() == "FIRE")
            {
                Debug.Log("Wood Left Fire");
                collider.gameObject.GetComponent<Torch>().LitState = "isLit";
            }
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerWithLight"))
        {
            if (collider.gameObject.GetComponentInChildren<Item>())
                if (collider.gameObject.GetComponentInChildren<Item>().SourceType.ToUpper() == "FIRE")
                {                    
                    Debug.Log("Wood being held Leaves Flame");
                    collider.gameObject.GetComponentInChildren<Torch>().LitState = "isLit";
                }
        }

    }
}
