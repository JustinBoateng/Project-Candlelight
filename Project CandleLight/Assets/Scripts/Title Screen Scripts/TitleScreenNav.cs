using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenNav : MonoBehaviour
{
    public GameObject FileSelectPanel;
    public GameObject FirstButton;

    // Start is called before the first frame update
    void Start()
    {
        HideFilePanel();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(FirstButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFilePanel()
    {
        FileSelectPanel.SetActive(true);
    }
    //load button activates this

    public void HideFilePanel()
    {
        FileSelectPanel.SetActive(false);
    }
    //new button activates this... for now

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
