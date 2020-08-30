using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSources : MonoBehaviour
{
    public bool Lit = true;
    public CircleCollider2D ShineDetect;
    public Light2D Illuminescense;
    public bool canShine = true; //if the source is intact or not clogged.
    public BoxCollider2D SourceTriggerBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
