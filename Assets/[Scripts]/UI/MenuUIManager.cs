//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 3, 2021
//      File            : MenuUIManager.cs
//      Description     : This script contains methods used on the main menu scene.
//      History         :   v0.5 - Added methods to the corresponding button presses.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    /// <summary>
    /// Each playmode button passes in a bool to the gamemanager to set the type of game to be played(timed or unlimited).
    /// </summary>
    /// <param name="trufal"></param>
    public void OnPlayModeButtonpressed(bool trufal)
    {
        if (trufal)
        {
            GameManager.Instance.unlimited = true;
            SceneManager.LoadScene("PlayScene");
            GameManager.Instance.timer = 0;
        }
        else
        {
            SceneManager.LoadScene("PlayScene");
            GameManager.Instance.timer = 301;
        }
    }
    /// <summary>
    /// Button to quit the game
    /// </summary>
    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
