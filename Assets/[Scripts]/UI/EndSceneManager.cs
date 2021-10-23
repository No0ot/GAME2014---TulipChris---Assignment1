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
