using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    public Flame[] TorchList = new Flame[4];

    public Door[] Doors = new Door[2];

    public bool Open = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < TorchList.Length; i++)
        {
            if(!TorchList[i].LitState)
            {
                Open = false;
                break;
            }
            Open = true;
        }

        if (Open) for(int i = 0; i<Doors.Length; i++) Doors[i].setLock(false);
        else for (int i = 0; i < Doors.Length; i++) Doors[i].setLock(true);
    }
}
