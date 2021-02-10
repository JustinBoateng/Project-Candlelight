using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is used mostly as a way to custom configure buttons.
//the other two classes needed are Movement (for player) and  MenuNavigation
/*
BTW, the purpose of GM is for other classes to be able to reference this GameManager

See, since GameManager is static, 
that means it's a class that can be referenced by other classes 
without having to make a public field or a serialized field to reference them
...That is, as long as this is the only GameManager of it's kind. 
Making more than one would make figuring out which gamemanager to reference confusing 

But we try to make sure here that there exists one instance of a GameManager (GM) 
through the Singleton Check Pattern in Awake 
        (And we use Awake() and not Start() 
        because we want the Singleton Check Pattern code to run at the beginning of the game's runtime
        even if, for some reason, the current GM is not active)
So that other classes know for sure which one to reference.

TL;DR: GameManager references the class
       GameManager.GM references
       THIS
       SINGLE
       SPECIFIC
       Game Object that is a GameManager.
       And that is why you will see other classes reference this object as GameManager.GM and not GameManager
*/

public class GameManager : MonoBehaviour 
{
    public static GameManager GM;

    public KeyCode jump { get; set;}
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode down { get; set; }
    public KeyCode up { get; set; }

    /*
     depending on if we're getting jump (x = GM.jump)
     or setting it (jump = x)
     we can impose certain rules
     that's the point of these {get;set;} brackets
         */


    private void Awake()
    {
      
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
    
        else if (GM != this)
            Destroy(gameObject);
       


        jump = (KeyCode) System.Enum.Parse(
                            typeof(KeyCode), 
                            PlayerPrefs.GetString("jumpKey", "Space")
                         );
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        


    }
    //Singleton Pattern. Basically guaranteeing that there can only be one GameManager in existance at a time.
    //If no GameManager exists, assign the GM to THIS INSTANCE of a script. 
    //We dont want it to destroy on load so that it can persist through all of our scenes.
    //If the GameManager exists but it's not equal to THIS SPECIFIC INSTANCE of the script, destroy it.
    //////////////////////////////////////////////////////////////////////////////////////////////////////////

    //We're going to use PlayerPreferences, which saves data on the local machine for a user, to grab a player's specific input for a key they registered, based on the last time they played the game
    //We'll also assign a default key. For Example, if a default jump button doesn't already exist, then we'll set the jump button to the space bar ("Space")
    //What "jump" is being assigned to is a KeyCode casted use of System.Enum.Parse, 
    //Which is a C# generic function that basically assigns PlayerPre--whatever that is, to a "type of" Keycode

    //When Awake runs, all these values will be assigned based on PlayerPreferences, or by the defaults you've specified.
    //The strings (upKey, downKey,etc...) are set in ... 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
