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

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        UpdateResourceBar();
        UpdateObjectivePanel();
    }

    void UpdateResourceBar()
    {
        goldResourceText.SetText(playerRef.gold.ToString());
        ironResourceText.SetText(playerRef.iron.ToString());
        steelResourceText.SetText(playerRef.steel.ToString());
    }

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

    void UpdateObjectivePanel()
    {
        FormatTimeText();
        killsText.SetText(playerRef.totalKills.ToString());
    }

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

    public void SetMasterVolume(float volume)
    {
        SoundManager.Instance.mixergroup[0].audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.mixergroup[1].audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        SoundManager.Instance.mixergroup[2].audioMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
        SoundManager.Instance.mixergroup[3].audioMixer.SetFloat("EnemyVolume", Mathf.Log10(volume) * 20);
        SoundManager.Instance.mixergroup[4].audioMixer.SetFloat("TowerVolume", Mathf.Log10(volume) * 20);
    }
}
