//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script to handle global gameplay variables such as the game timer and player kills to transfer these variables between scenes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public float timer;

    public bool unlimited = false;

    public int lives = 3;

    public bool lifeRefresh = true;
    float lifeTimer;
    float lifeTimerMax = 60;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimerCountdown();
        if(lives < 3 && lifeRefresh)
        {
            Debug.Log("life regen");
            StartCoroutine(LifeRegenerate());
        }
    }

    void TimerCountdown()
    {
        if (!unlimited)
            timer -= Time.deltaTime;
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
}
