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
            }
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(collider.gameObject.GetComponentInChildren<Item>())
                if (collider.gameObject.GetComponentInChildren<Item>().SourceType.ToUpper() == "FIRE")
                    Debug.Log("Torch Detects Wood in Child");
        }
    }
}
