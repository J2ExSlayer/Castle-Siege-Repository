//Taylor, Madi
//10/30/2024
//Handles Game SceneSwitch
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

