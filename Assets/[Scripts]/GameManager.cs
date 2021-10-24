//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 23, 2021
//      File            : GameManager.cs
//      Description     : Singleton class for game related properties
//      History         :   v0.5 - Added timer functionality and game over state.
//                          v0.7 - Added regenerating lives functionality
//                          v1.0 - Added difficulty increase implementation
//
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
    /// <summary>
    /// Sets music when a specific level is loaded along with the starting gameplay variables for the gameplay scene
    /// </summary>
    /// <param name="level"></param>
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
    /// <summary>
    /// Simple function for Timer functionality
    /// </summary>
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
    /// <summary>
    /// Co-routine started once the player loses a life. When completed regenerates a life.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Checks the players life to see if it needs to Game Over.
    /// </summary>
    public void CheckLives()
    {
        if (lives <= 0)
        {
            GameOver();
        }
    }
    /// <summary>
    /// Game over Function used to load the end scene and pass in the total kills to be displayed there.
    /// </summary>
    public void GameOver()
    {
        finalKills = PlayerStats.Instance.totalKills;
        SceneManager.LoadScene("EndScene");
    }
    /// <summary>
    /// Every 30 seconds the difficulty of the game increases by incrementing the health of enemies and decrementing the time between enemy spawns.
    /// </summary>
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
    /// <summary>
    /// At specific times a new enemy will start spawning.
    /// </summary>
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
