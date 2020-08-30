using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuNavigation : MonoBehaviour {

    // Use this for initialization
    public static bool GameIsPaused;

    public GameObject pauseMenuUI;

    private void Start()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused) Resume();
            else Pause();

        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
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
