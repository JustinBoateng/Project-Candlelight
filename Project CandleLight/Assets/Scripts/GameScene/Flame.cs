using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Items"))
        {
            if (collider.gameObject.GetComponent<Item>().SourceType.ToUpper() == "FIRE")
            {
                Debug.Log("Torch Detects Wood");
                collider.gameObject.GetComponent<Torch>().LitState = "isInFire";
            }
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collider.gameObject.GetComponentInChildren<Item>())
                if (collider.gameObject.GetComponentInChildren<Item>().SourceType.ToUpper() == "FIRE")
                {
                    Debug.Log("Torch Detects Wood in Child");
                    collider.gameObject.GetComponentInChildren<Torch>().LitState = "isInFire";
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

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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
