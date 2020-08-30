using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour

{
    public bool MasterLights = true;
    public bool MonsterDetect = false;
    public LightSources[] LightSystem = new LightSources[10];
    public Vent[] VentSystem = new Vent[10];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMasterLights()
    {
        MasterLights = !MasterLights;
    }

    public bool hasMonster()
    {
        return MonsterDetect;
    }

}
