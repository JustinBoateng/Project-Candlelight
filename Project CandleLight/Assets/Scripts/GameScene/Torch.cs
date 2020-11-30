using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Torch : MonoBehaviour
{
    public float maxEndurance = 120;
    public float FireEndurance = 0;
    public float EndurRate = 0;
    public LightTrigger LightArea;
    public Sprite[] TorchImages = new Sprite[3];

    public string LitState = "notLit";
    public bool isUsed = false;
 
    //0 = not Lit
    //1 = is Lit    
    //2 = was used
    // Start is called before the first frame update
    void Start()
    {
        LitState = "notLit";
        InvokeRepeating("EnduranceCalc", 2.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        TorchFunction(LitState);
        
        if (FireEndurance < 0) FireEndurance = 0;

        //if lightarea's state changes, call Lightsystem's MinorLight'sStateCheck

        if (LightSystem.LS.MasterLightSystem.gameObject.activeSelf)
        {
            LightArea.gameObject.SetActive(false);
            LightSystem.LS.MinorLightsStateCheck();
        }
        //if the master light is on, then forget everything else about the torches, just keep track of the litState

        else if (FireEndurance == maxEndurance && LitState == "isInFire")
        {
            LightArea.gameObject.SetActive(true);
            LightSystem.LS.MinorLightsStateCheck();
            GetComponentInParent<SpriteRenderer>().sprite = TorchImages[1];
        }
        else if ((FireEndurance > 0 && FireEndurance <= maxEndurance) && LitState == "isLit")
        {
            LightArea.gameObject.SetActive(true);
            LightSystem.LS.MinorLightsStateCheck();
            GetComponentInParent<SpriteRenderer>().sprite = TorchImages[1];
        }
        else if (FireEndurance == 0)
        {
            LightArea.gameObject.SetActive(false);
            LightSystem.LS.MinorLightsStateCheck();
            if (isUsed) GetComponentInParent<SpriteRenderer>().sprite = TorchImages[2];
            else GetComponentInParent<SpriteRenderer>().sprite = TorchImages[0];
        }
        else if ((FireEndurance >= 0 && FireEndurance < maxEndurance) && (LitState == "notLit"))
        {
            LightArea.gameObject.SetActive(false);
            LightSystem.LS.MinorLightsStateCheck();
            if (isUsed) GetComponentInParent<SpriteRenderer>().sprite = TorchImages[2];
            else GetComponentInParent<SpriteRenderer>().sprite = TorchImages[0];
        }

    }

    void TorchFunction(string LitState) {

        //LitState changes according to interactables interacting with the Torch Code
        //A.K.A. Outside forces are what adjust the LitState

        switch (LitState)
        {
            case "notLit":
                FireEndurance = 0;
                EndurRate = 0;
                break;

            case "isLit":
                EndurRate = 1;
                break;
                
            case "cannotBeLit":
                FireEndurance = 0;
                EndurRate = 0;
                break;



            case "isInFire":
                FireEndurance = maxEndurance;
                EndurRate = 0;
                isUsed = true;
                break;

            case "isInWind":
                EndurRate = 3;
                break;
        }
    }


    void EnduranceCalc()
    {
        if (FireEndurance == 0) return;
        FireEndurance = FireEndurance - EndurRate;
        return;
    }
}
