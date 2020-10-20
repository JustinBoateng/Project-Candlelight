using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour {

    // Use this for initialization
    public static bool GameIsPaused;

    public GameObject pauseMenuUI;

    public GameObject DefaultButton;

    public EventSystem ES;

    [SerializeField]
    public Text DescriptionSpace;

    // public Inventory PauseInv;

    private void Start()
    {
        Resume();
        ES.SetSelectedGameObject(DefaultButton);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused) Resume();
            else Pause();

        }

        if (ES.currentSelectedGameObject.GetComponent<Item>()) DescriptionSpace.text = ES.currentSelectedGameObject.GetComponent<Item>().description;
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //PauseInv.ItemMenuClear();
        ES.SetSelectedGameObject(null);
        DescriptionSpace.text = "";
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //PauseInv.ItemFill();
        ES.SetSelectedGameObject(DefaultButton);
    }

    public void LoadOptions()
    {
        Debug.Log("Loading Options...");
        //SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SaveGame()
    {
        Debug.Log("Saving Game...");
    }

}
