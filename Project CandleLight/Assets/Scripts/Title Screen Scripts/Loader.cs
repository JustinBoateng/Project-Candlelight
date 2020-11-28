using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum Scene {
        GameScene,
    }

    public void Load()
    {
        SceneManager.LoadScene(1);
    }

}
