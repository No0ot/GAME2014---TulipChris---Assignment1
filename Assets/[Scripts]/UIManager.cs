using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
            GameManager.Instance.timer = 300;
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
