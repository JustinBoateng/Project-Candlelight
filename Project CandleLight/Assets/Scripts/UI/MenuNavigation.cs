using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/*Attached to the canvas
    Toggles 
        >The visibility and interactivity of the pause menu
        >Whether or not the game world is moving or not
        >Button Functionality
        >Screen Size
        >Exiting the game

    Note: the Inventory Class deals with items in the menu. NOT this (MenuNavigation) class. MenuNavigation deals with moving through the main menu
    Inventory can SPAWN buttons that can be navigated through EventSystem as buttons do. 
    MenuNavigation also helps attach those items to the description box.


    ES (EventSystem) is taken into consideration so that we can
        >Default the first button to be the "Resume" button,
         so that you dont need a mouse to click a button
        >So that when you hover over an Item in inventory, you can take the description of the item and automatically 
         post it in the DescriptionSpace (Description Box)

    We placed this class in "Canvas" and NOT in "PlayerMenu" because
        >This class, when "PauseButton" is pressed, toggles the active value of what is in the pauseMenuUI reference,
         In this case BEING the "PlayerMenu" itself.
         "PlayerMenu" cannot untoggle itself if it inactive
    It IS possible to place this class in another constantly active object, but the Canvas can fit the roll without much conflict for now.
*/

public class MenuNavigation : MonoBehaviour {

    public static MenuNavigation MN;

    // Use this for initialization
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;

    public GameObject DefaultButton;
    public EventSystem ES;
    [SerializeField] public Text DescriptionSpace;

    public GameObject[] Tab = new GameObject[3]; //0 = Inventory, 1 = Options, 2 = Quit
    public Button[] MenuButton = new Button[4]; //0 = Resume, 1 = Inventory, 2 = Options , 3 = Quit 
    /*
        public void tabUpdate(bool ITabActive, bool OTabActive, bool QTabActive)
        {
            Tab[0].setActive = ITabActive;
            Tab[1].setActive = OTabActive;
            Tab[2].setActive = QTabActive;
        }
        
        if (MenuButton[1] -- Inventory) is pressed, run tabUpdate(true, false, false)
        if (MenuButton[2] -- Options) is pressed, run tabUpdate(false, true, false)
        if (MenuButton[3] -- Quit) is pressed, run tabUpdate(false, false, true)
        if (MenuButton[0] -- Resume) is pressed, run Resume();


        Also, quitting the game sends you to the Title Scene
    */

    //This stuff is SPECIFICALLY for Button Configurations
    
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey; //a flag to note when the player is pressing a button

    public Transform ButtonConfigPanel;
    public Transform buttonPressIndicator;

    //--------------------------------------------------------------------------------        

    public Toggle SaveFlag;
   
    public MiniInv MIReference;

    private void Start()
    {
       

        //this is basically Resume() but without the ButtonConfigPanel being touched so that Unity can register it to Canvas's MenuNavigation
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //PauseInv.ItemMenuClear();
        ES.SetSelectedGameObject(null);
        DescriptionSpace.text = "";
        //----------------------------------------------------------------

        ES.SetSelectedGameObject(DefaultButton);
        MenuButton[0].onClick.AddListener(() => Resume()); //Resume Button

        //tab navigation------------------------------------------------------------------
        MenuButton[0].onClick.AddListener(delegate { tabUpdate(true, false, false); }); //and also reset the toggled tab to inventory
        MenuButton[1].onClick.AddListener(delegate { tabUpdate(true, false, false); }); //Inventory toggle
        MenuButton[2].onClick.AddListener(delegate { tabUpdate(false, true, false); }); //Inventory toggle
        MenuButton[3].onClick.AddListener(delegate { tabUpdate(false, false, true); }); //Inventory toggle
        tabUpdate(true, false, false);
        //--------------------------------------------------------------------------------


        //Button Configuration Section----------------------------------------------------
        buttonPressIndicator = transform.Find("Button Check Panel");
        buttonPressIndicator.gameObject.SetActive(false);

        ButtonConfigPanel = transform.Find("Button Config"); //Get the buttonconfig panel from the child of the object this script is attached to (AKA the Canvas). 
                                                             //Note: As of writing this, buttonconfig is the child of a child of a child of the canvas. Perhaps move Button Config to be a direct child of Canvas if you get any problems
        ButtonConfigPanel.gameObject.SetActive(false);       //make sure it's inactive   
        waitingForKey = false;                               //initialize waitingForKey flag to false


        for (int i = 0; i < 5; i++)
        {
            ColorBlock Sample = new ColorBlock();

            //Debug.Log(ButtonConfigPanel.GetChild(i).name);

            if (ButtonConfigPanel.GetChild(i).name == "upKey")
                ButtonConfigPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GM.up.ToString();
            else if (ButtonConfigPanel.GetChild(i).name == "downKey")
                ButtonConfigPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GM.down.ToString();
            else if (ButtonConfigPanel.GetChild(i).name == "leftKey")
                ButtonConfigPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GM.left.ToString();
            else if (ButtonConfigPanel.GetChild(i).name == "rightKey")
                ButtonConfigPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GM.right.ToString();
            else if (ButtonConfigPanel.GetChild(i).name == "jumpKey")
                ButtonConfigPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GM.jump.ToString();

            GameObject Reference = ButtonConfigPanel.GetChild(i).gameObject;


            ButtonConfigPanel.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
            {



                Debug.Log("Running SentText");
                SentText(Reference.GetComponentInChildren<Text>());

                ES.SetSelectedGameObject(null);

                Debug.Log("Running StartAssignment with " + Reference.name.Replace("Key", ""));
                StartAssignment(Reference.name.Replace("Key", ""));

                
                ES.SetSelectedGameObject(Reference); //set the selected game object back to the button

            });
        
        }
        //giving the buttons for button config functionality
        //--------------------------------------------------------------------------------

        MN = this;
        //be sure to initialize this instance of the MenuNavigation for other classes to reference freely 
    }

    //it works if we use the mouse
    //but struggles with keyboard only
    //also, you gotta figure out how to properly assign colors...

    /*
        setting up button functionality for each of them
        We're going to cycle through the children of the "Button Config" panel. Searching for buttons with the "...Key" name
        if we meet a button, we're going to take it's text (which will be a child of that button) and reference it to the GameManager (GM) (We can do this since the GameManager is static. Meaning it's only one instance of it, and other classes can reference it freely without having to make a variable to reference it through)
        In this case, we're going to assign that text to the GameManager's respective button's Keycode, casted as a string using the ToString function
        Ex: if GM.up's KeyCode is "W", which it is by default, then ToString will assign "W" to be the text of the ButtonConfig's "upKey" Button
        we do this at start because we want SOMETHING to be in those buttons. Most likely the Default Keycodes given it's their first time opening the menu up
    */

    /*
       Sample.normalColor = new Color(183, 47, 47);
       Sample.disabledColor = new Color(255, 255, 255);
       Sample.pressedColor = Color.red; //new Color(183, 47, 47,1);
       Sample.highlightedColor = Color.cyan;//new Color(253,132,132,100);
       Sample.selectedColor = Color.cyan;//new Color(253, 132, 132);
       Sample.colorMultiplier = 1;
       Sample.fadeDuration = 0.1f;
       //Debug.Log(ButtonConfigPanel.GetChild(i).name);
       //Debug.Log(Sample.normalColor.ToString());
       Reference.GetComponent<Button>().colors = Sample;
       */



    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
        //Pause/Resume toggle, as long as we're not changing buttons

        if (GameIsPaused)
        {
            if (ES.currentSelectedGameObject.GetComponent<Item>())
                DescriptionSpace.text = ES.currentSelectedGameObject.GetComponent<Item>().description;
        }
        else { }
        //Description box functionality

    }

    public void tabUpdate(bool ITabActive, bool OpTabActive, bool QuTabActive)
    {
        Tab[0].gameObject.SetActive(ITabActive);
        Tab[1].gameObject.SetActive(OpTabActive);
        Tab[2].gameObject.SetActive(QuTabActive);
    }

    public void Resume()
    {
        if (ButtonConfigPanel.gameObject.activeSelf) return;

        pauseMenuUI.SetActive(false);
        ButtonConfigPanel.gameObject.SetActive(false);
        buttonPressIndicator.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //PauseInv.ItemMenuClear();
        ES.SetSelectedGameObject(null);
        DescriptionSpace.text = "";
        MIReference.gameObject.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        tabUpdate(true, false, false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        ES.SetSelectedGameObject(DefaultButton);
        MIReference.gameObject.SetActive(false);
    }

    public void LoadOptions()
    {
        Debug.Log("Loading Options...");
        //SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        if (SaveFlag.isOn) SaveGame();

        Debug.Log("Quitting Game...");     
        Application.Quit();
    }

    public void ReturntoTitleMenu()
    {
        if (SaveFlag.isOn) SaveGame();

        Debug.Log("Returning to Menu...");
        SceneManager.LoadScene("TitleScreen");
    }

    public void SaveGame()
    {
        Debug.Log("Saving Game...");
    }


    //Button Configuration Functions--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------

    public void OnGUI() 
    {
        keyEvent = Event.current; //keyEvent will gain Event.Current (the event that is currently happening in the GUI during that frame)
        if (keyEvent.isKey  && waitingForKey) //if there is a keyboard input and we are currently waiting for a key
        {            
                newKey = keyEvent.keyCode;
                waitingForKey = false;            
        }

    }
     // if the current event happening is that of a keyboard event...
     // then we'll have newKey take that keyboard event and register what key is being pressed
     //...and set waitingForKey back to false

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }
     //Think of Coroutines as functions that start at intervals, over and over again
     //Buttons cannot call on coroutines in UnityEditor, so you'll have to code them in yourself
     //Instead, the buttons will call on this StartAssignment function, which will instead call the coroutine

    public void SentText(Text text)
    {
        buttonText = text;
    }
    /*this is used to give the player feedback that their new assigned button has been registered,
    by changing the text of the button in question.
    This will be called by the button itself when clicked 
    and will send in the text argument that will likely be a child of the button
    so that the text itself can be modified by the coroutine 
    and therefore modified (as a consequence/feedback of setting controls) by the player
    
    NOTE: When applying the functions to the button, make sure to set SendText first, THEN StartAssignment     
         
    */

    IEnumerator WaitForKey()
    {
        ES.SetSelectedGameObject(null);
        Debug.Log("In WaitForKey() waiting for an input");
        //while (!keyEvent.isKey || Input.GetKey(KeyCode.Return))// || Input.GetKeyUp(KeyCode.Return))
        //    yield return null;

        yield return new WaitWhile(() => !keyEvent.isKey || Input.GetKey(KeyCode.Return));

        Debug.Log("Input received");

    }
    //this will be a control statement in our other coroutine
    //if the event in the GUI is not that of a keyboard input, then ignore it.
    //don't worry, the only way this coroutine will be called in the firsy place is if waitingForKey is set to true, i.e. if the game is waiting for a keyboard input from the player.

    public IEnumerator AssignKey(string keyName)
    {
        Debug.Log("set up waitingForKey");
        waitingForKey = true;

        //pop up a screen to tell the player to press a button

        Debug.Log("The panel is open");
        buttonPressIndicator.gameObject.SetActive(true);

        //new WaitForSecondsRealtime(1);
        Debug.Log("we're running WaitForKey to wait for input");
        yield return WaitForKey();

        Debug.Log("Input received, setting up buttons");
        buttonPressIndicator.gameObject.SetActive(false);
        //remove the screen telling the player to press a button

        switch(keyName)
        {
            case "left":
                GameManager.GM.left = newKey;
                buttonText.text = GameManager.GM.left.ToString();
                PlayerPrefs.SetString("leftKey", GameManager.GM.left.ToString());
                break;

            case "right":
                GameManager.GM.right = newKey;
                buttonText.text = GameManager.GM.right.ToString();
                PlayerPrefs.SetString("rightKey", GameManager.GM.right.ToString());
                break;

            case "up":
                GameManager.GM.up = newKey;
                buttonText.text = GameManager.GM.up.ToString();
                PlayerPrefs.SetString("upKey", GameManager.GM.up.ToString());
                break;

            case "down":
                GameManager.GM.down = newKey;
                buttonText.text = GameManager.GM.down.ToString();
                PlayerPrefs.SetString("downKey", GameManager.GM.down.ToString());
                break;

            case "jump":
                GameManager.GM.jump = newKey;
                buttonText.text = GameManager.GM.jump.ToString();
                PlayerPrefs.SetString("jumpKey", GameManager.GM.jump.ToString());
                break;


        }
        yield return null; 
    }
    /*basically stop the coroutine from executing until a keyboard input is taken in. 
      WaitForKey's while loop will loop infinitely until a keyboard input is taken in
      when a keyboard input is taken in, OnGUI (which is running every frame for GUI specific events) 
      will notice that keyboard input as the Event Currently Happening.
      This, plus waitingForKey being set to true due to AssignKey() fulfills OnGUI's given IF statement
      will successfully set the newKey to the Keyboard's input at the appropriate time
      after which, OnGUI will set waitingForKey to false
     
      As for the switch statment
      
      Here's an example:  
      given that our keyname (the name of the key we're modifying) is "left":

      >Assign GM's (our current, single, specific GameManager) 
       left keyCode to whatever the newKey (the input given on the keyboard) is  
      >set the buttontext of the button we're currently modifying to what was just pressed 
      so that the feedback of the change is given to the player through the text changing
      >set the PlayerPrefs in Unity to the controls mentioned. 
       This allows their preferences to be saved when they close their game/ open their game
       Since GM's Awake() will fetch that given key instead of the default it was assigned. 
     
      The yield return null at the end is unnecessary. Just want to let the script wait a frame before execution ends.
     */

    /*
    public void StartAssignmentPartOne(string keyName)
    {

        Debug.Log("The panel is open");
        buttonPressIndicator.gameObject.SetActive(true);

        //pop up a screen to tell the player to press a button

        
      
    }

    //if partition is EVER true run this using "carryover" as the perimeter

    public void StartAssignmentPartTwo(string keyName)
    {

        Debug.Log("set up waitingForKey");
        waitingForKey = true;

        //new WaitForSecondsRealtime(1);
        Debug.Log("we're running WaitForKey to wait for input");
        WaitForKey();

        Debug.Log("Input received, setting up buttons");
        buttonPressIndicator.gameObject.SetActive(false);
        //remove the screen telling the player to press a button

        switch (keyName)
        {
            case "left":
                GameManager.GM.left = newKey;
                buttonText.text = GameManager.GM.left.ToString();
                PlayerPrefs.SetString("leftKey", GameManager.GM.left.ToString());
                break;

            case "right":
                GameManager.GM.right = newKey;
                buttonText.text = GameManager.GM.right.ToString();
                PlayerPrefs.SetString("rightKey", GameManager.GM.right.ToString());
                break;

            case "up":
                GameManager.GM.up = newKey;
                buttonText.text = GameManager.GM.up.ToString();
                PlayerPrefs.SetString("upKey", GameManager.GM.up.ToString());
                break;

            case "down":
                GameManager.GM.down = newKey;
                buttonText.text = GameManager.GM.down.ToString();
                PlayerPrefs.SetString("downKey", GameManager.GM.down.ToString());
                break;

            case "jump":
                GameManager.GM.jump = newKey;
                buttonText.text = GameManager.GM.jump.ToString();
                PlayerPrefs.SetString("jumpKey", GameManager.GM.jump.ToString());
                break;


        }

        Partition = false;
        return;
    }
    */
    //--------------------------------------------------------------------------------        
    //--------------------------------------------------------------------------------


    //Screen Size Functions--------------------------------------------------------------------------------        
    //--------------------------------------------------------------------------------        

    public void setQuality()
    {
        SetScreenRes();
    }

    void SetScreenRes()
    {
        //getting the name of the button we pressed
        string index = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
        switch (index)
        {
            case "0":
                Screen.SetResolution(840, 448, false);
                break;
            //fullscreen

            case "1":
                Screen.SetResolution(1200, 768, false);
                break;
            //fullscreen

            case "2":
                Screen.SetResolution(1360, 796, false);
                break;
            //fullscreen

            case "3":
                Screen.SetResolution(1980, 1000, false);
                break;
            //fullscreen



        }

    }
    //--------------------------------------------------------------------------------        
    //--------------------------------------------------------------------------------        

    public bool isPaused()
    {
        return GameIsPaused;
    }

}
