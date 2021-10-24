//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : EndSceneManager.cs
//      Description     : This script contains methods used in the UI for the end scene.
//      History         :   v0.7 - Added Methods to interact with the end scene buttons
//                          v0.9 - Added Sounds to UI objects.
//                          
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text killsText;
    private void Start()
    {
        FormatTimeText();
        killsText.SetText(GameManager.Instance.finalKills.ToString());
    }
    /// <summary>
    /// Copy of the function from the main menu script with added sound functionality.
    /// </summary>
    /// <param name="trufal"></param>
    public void OnPlayModeButtonpressed(bool trufal)
    {
        if (trufal)
        {
            SoundManager.Instance.PlayRandomClickForward();
            GameManager.Instance.unlimited = true;
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SoundManager.Instance.PlayRandomClickForward();
            SceneManager.LoadScene("PlayScene");
            GameManager.Instance.timer = 301;
        }
    }
    /// <summary>
    /// Button pressed event to return to the main menu.
    /// </summary>
    public void MainMenuButtonPressed()
    {
        SoundManager.Instance.PlayRandomClickBackward();
        Destroy(GameManager.Instance.gameObject);
        Destroy(SoundManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// Copy of the format time text for the display of the final game time.
    /// </summary>
    private void FormatTimeText()
    {
        float minutes = (int)(GameManager.Instance.timer / 60) % 60;
        float seconds = (int)(GameManager.Instance.timer % 60);

        if (minutes < 1 && seconds < 10)
        {
            timeText.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else if (minutes < 1 && seconds >= 10)
        {
            timeText.text = "0" + minutes.ToString() + ":" + seconds.ToString();
        }
        else if (minutes >= 1 && seconds < 10)
        {
            timeText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            timeText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }
}
