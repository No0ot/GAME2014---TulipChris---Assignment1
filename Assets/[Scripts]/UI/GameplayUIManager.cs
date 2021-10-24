//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : GameplayUIManager.cs
//      Description     : This script contains methods used in the UI for the gameplay scene
//      History         :   v0.5 - Added methods to update the Resource and Objective Panel. Pause button functionality added as well as updates to the life sprites.
//                          v0.7 - Added Sound options to reduce the volume of Master/Music/Gameplay sounds.
//                          v0.9 - Added error prompt when player needs additional resources
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    private static GameplayUIManager instance;
    public static GameplayUIManager Instance { get { return instance; } }

    public TMP_Text goldResourceText;
    public TMP_Text ironResourceText;
    public TMP_Text steelResourceText;
    public TMP_Text timerText;
    public TMP_Text killsText;

    public PlayerStats playerRef;
    [SerializeField]
    private GameObject[] livesSprite;

    public DetailsPanel detailsPanel;
    public TDGrid gameplayGrid;

    public GameObject errorText;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        UpdateResourceBar();
        UpdateObjectivePanel();
    }
    /// <summary>
    /// Updates the resource bar.
    /// </summary>
    void UpdateResourceBar()
    {
        goldResourceText.SetText(playerRef.gold.ToString());
        ironResourceText.SetText(playerRef.iron.ToString());
        steelResourceText.SetText(playerRef.steel.ToString());
    }
    /// <summary>
    /// Formats the the text for the timer.
    /// </summary>
    private void FormatTimeText()
    {
        float minutes = (int)(GameManager.Instance.timer / 60) % 60;
        float seconds = (int)(GameManager.Instance.timer % 60);

        if (minutes < 1 && seconds < 10)
        {
            timerText.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else if (minutes < 1 && seconds >= 10)
        {
            timerText.text = "0" + minutes.ToString() + ":" + seconds.ToString();
        }
        else if (minutes >= 1 && seconds < 10)
        {
            timerText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            timerText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }
    /// <summary>
    /// Updates the objective panel.
    /// </summary>
    void UpdateObjectivePanel()
    {
        FormatTimeText();
        killsText.SetText(playerRef.totalKills.ToString());
    }
    /// <summary>
    /// Pauses the game. Game cannot be unpaused if the player has not built a path.
    /// </summary>
    /// <param name="trufal"></param>
    public void PauseButton(bool trufal)
    {
        if (trufal && gameplayGrid.endTile)
            Time.timeScale = 1.0f;
        else
        {
            Time.timeScale = 0.0f;
            transform.GetChild(4).GetComponent<Toggle>().isOn = false;
        }
    }
    /// <summary>
    /// Updates the Sprites used to symbolize the players lives.
    /// </summary>
    public void UpdateLifeSprites()
    {
        switch(GameManager.Instance.lives)
        {
            case 0:
                livesSprite[0].GetComponent<Image>().color = new Color(0, 0, 0);
                break;
            case 1:
                livesSprite[0].GetComponent<Image>().color = new Color(255, 255, 255);
                livesSprite[1].GetComponent<Image>().color = new Color(0, 0, 0);
                livesSprite[2].GetComponent<Image>().color = new Color(0, 0, 0);
                break;
            case 2:
                livesSprite[1].GetComponent<Image>().color = new Color(255, 255, 255);
                livesSprite[2].GetComponent<Image>().color = new Color(0, 0, 0);
                break;
            case 3:
                livesSprite[2].GetComponent<Image>().color = new Color(255, 255, 255);
                break;
        }
    }
    /// <summary>
    /// Sets Master Volume based on a slider
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        SoundManager.Instance.mixergroup[0].audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    /// <summary>
    /// Sets the Music volume based on a slider
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.mixergroup[1].audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    /// <summary>
    /// Sets the gameplay effects volume based on a slider.
    /// </summary>
    /// <param name="volume"></param>
    public void SetEffectsVolume(float volume)
    {
        SoundManager.Instance.mixergroup[2].audioMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
        SoundManager.Instance.mixergroup[3].audioMixer.SetFloat("EnemyVolume", Mathf.Log10(volume) * 20);
        SoundManager.Instance.mixergroup[4].audioMixer.SetFloat("TowerVolume", Mathf.Log10(volume) * 20);
    }
    /// <summary>
    /// Displays the passed in text to the player on the screen
    /// </summary>
    /// <param name="text"></param>
    public void DisplayErrorText(string text)
    {
        SoundManager.Instance.PlayErrorSound();
        errorText.SetActive(true);
        errorText.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        StartCoroutine(ErrorTextDisplaylength());
    }
    /// <summary>
    /// Co-Routine used for the amount of time to display the error message.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ErrorTextDisplaylength()
    {
        float timer = 0;
        float timerMax = 3;
        do
        {
            //timer += Time.deltaTime;
            timer += 0.01f;
            yield return null;

        } while (timer < timerMax);

        if (timer >= timerMax)
        {
            errorText.SetActive(false);
            StopCoroutine(ErrorTextDisplaylength());
            yield return null;
        }

    }
}
