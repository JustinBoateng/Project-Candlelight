using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable {

    public BoxCollider2D[] Platforms;
    public bool platformsactive = true;

    // Start is called before the first frame update
    void Start()
    {
        this.InterType = "Ladder";

    }

    public void PlatformToggle()
    {
        platformsactive = !platformsactive; 
        for (int i = 0; i <= Platforms.Length - 1; i++)
        {
            Platforms[i].gameObject.SetActive(!Platforms[i].enabled);
        }
    }
}
