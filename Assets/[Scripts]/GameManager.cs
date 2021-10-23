//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script to handle global gameplay variables such as the game timer and player kills to transfer these variables between scenes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public float timer;
    float difficultyTimer;
    float newUnitTimer;
    public bool unlimited = false;

    public int lives = 3;

    public bool lifeRefresh = true;
    float lifeTimer;
    float lifeTimerMax = 60;

    public float finalKills;
    public bool inGame;

    public EnemyWaveSpawner enemySpawner;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    private void OnLevelWasLoaded(int level)
    {
        switch(level)
        {
            case 0:
                SoundManager.Instance.PlayMenuMusic();
                break;
            case 1:
                SoundManager.Instance.PlayGameplayMusic();
                Time.timeScale = 0.0f;
                enemySpawner = PlayerStats.Instance.enemySpawner;
                finalKills = 0;
                difficultyTimer = 0;
                newUnitTimer = 0;
                break;
            case 2:
                StopAllCoroutines();
                SoundManager.Instance.PlayMenuMusic();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "PlayScene")
        {
            TimerCountdown();
            if (lives < 3 && lifeRefresh)
            {
                Debug.Log("life regen");
                StartCoroutine(LifeRegenerate());
            }

            TimedDifficultyIncrease();
            NewUnitEnabled();
        }
    }

    void TimerCountdown()
    {
        if (!unlimited)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameOver();
            }
        }
        else
            timer += Time.deltaTime;
    }

    private IEnumerator LifeRegenerate()
    {
        lifeTimer = 0;
        lifeRefresh = false;
        do
        {
            lifeTimer += Time.deltaTime;
            yield return null;

        } while (lifeTimer < lifeTimerMax);

        if (lifeTimer >= lifeTimerMax)
        {
            lives++;
            GameplayUIManager.Instance.UpdateLifeSprites();
            StopCoroutine(LifeRegenerate());
            lifeRefresh = true;
            yield return null;
        }

    }

    public void CheckLives()
    {
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        finalKills = PlayerStats.Instance.totalKills;
        SceneManager.LoadScene("EndScene");
    }

    void TimedDifficultyIncrease()
    {
        if (difficultyTimer >= 30f)
        {
            enemySpawner.IncrementHealth();
            enemySpawner.DecrementTimer();
            difficultyTimer = 0f;
        }
        else
            difficultyTimer += Time.deltaTime;
    }

    void NewUnitEnabled()
    {
        if (newUnitTimer >= 30f && !enemySpawner.fastActive)
            enemySpawner.fastActive = true;
        if (newUnitTimer >= 60f && !enemySpawner.armoredActive)
            enemySpawner.armoredActive = true;
        if (newUnitTimer >= 210f && !enemySpawner.tankActive)
            enemySpawner.tankActive = true;

        newUnitTimer += Time.deltaTime;
    }

}
