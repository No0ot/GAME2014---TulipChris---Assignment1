//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script to handle all menu buttons for the main menu and end scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{

    public void OnPlayModeButtonpressed(bool trufal)
    {
        if (trufal)
        {
            GameManager.Instance.unlimited = true;
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SceneManager.LoadScene("PlayScene");
            GameManager.Instance.timer = 301;
        }
    }

    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPlayToggle(bool trufal)
    {
        if (trufal)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
