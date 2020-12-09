using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightTrigger : MonoBehaviour
{
    public Collider2D LightArea;
    public UnityEngine.Experimental.Rendering.Universal.Light2D Illumination;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Light";
        gameObject.layer = LayerMask.NameToLayer("Light");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
