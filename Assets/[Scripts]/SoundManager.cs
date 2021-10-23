using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource menuMusicSource;
    public AudioSource bgMusicSource;
    public AudioSource uiSoundSource;
    List<AudioSource> enemyAudioSourceList;
    List<AudioSource> towerAudioSourceList;

    //Music Tracks
    public AudioClip menuMusic;
    public AudioClip gameplayTrack1;
    public AudioClip gameplayTrack2;

    //Grid Sounds
    public AudioClip digSound;
    public AudioClip errorSound;

    //UI sounds
    public AudioClip[] clickForward;
    public AudioClip[] clickBack;

    //Tower Sounds
    public AudioClip towerShootBasic;
    public AudioClip towerShootRapid;
    public AudioClip towerUpgrade;
    public AudioClip towerBuild;

    //Enemy Sounds
    public AudioClip[] enemyDie;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        menuMusicSource.clip = menuMusic;
        menuMusicSource.Play();
        enemyAudioSourceList = new List<AudioSource>();
        towerAudioSourceList = new List<AudioSource>();
    }
    private void Update()
    {
        if (!bgMusicSource.isPlaying)
        {
            if (bgMusicSource.clip == gameplayTrack1)
            {
                bgMusicSource.clip = gameplayTrack2;
                bgMusicSource.Play();
            }
            else if(bgMusicSource.clip == gameplayTrack2)
            {
                bgMusicSource.clip = gameplayTrack1;
                bgMusicSource.Play();
            }
        }
    }

    public AudioSource GetEnemyAudioSource()
    {
        foreach(AudioSource source in enemyAudioSourceList)
        {
            if(!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource temp = transform.GetChild(0).gameObject.AddComponent<AudioSource>();
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        enemyAudioSourceList.Add(temp);
        return temp;
    }

    public AudioSource GetTowerAudioSource()
    {
        foreach (AudioSource source in towerAudioSourceList)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource temp = transform.GetChild(1).gameObject.AddComponent<AudioSource>();
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        towerAudioSourceList.Add(temp);
        return temp;
    }

    public void PlayTowerBuild()
    {
        uiSoundSource.clip = towerBuild;
        uiSoundSource.Play();
    }

    public void PlayTowerUpgrade()
    {
        uiSoundSource.clip = towerUpgrade;
        uiSoundSource.Play();
    }

    public void PlayMenuMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = null;
        menuMusicSource.clip = menuMusic;
        menuMusicSource.Play();
    }

    public void PlayGameplayMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = gameplayTrack1;
        bgMusicSource.Play();
    }

    public void PlayDigSound()
    {
        uiSoundSource.clip = digSound;
        uiSoundSource.Play();
    }

    public void PlayErrorSound()
    {
        uiSoundSource.clip = errorSound;
        uiSoundSource.Play();
    }

    public void PlayRandomClickForward()
    {
        uiSoundSource.clip = clickForward[Random.Range(0, (clickForward.Length))];
        uiSoundSource.Play();
    }

    public void PlayRandomClickBackward()
    {
        uiSoundSource.clip = clickBack[Random.Range(0, (clickBack.Length))];
        uiSoundSource.Play();
    }

    public void PlayRandomEnemyDeathSound()
    {
        AudioSource temp = SoundManager.Instance.GetEnemyAudioSource();
        temp.clip = enemyDie[Random.Range(0, (enemyDie.Length))];
        temp.Play();
    }

    public void PlayTowerShoot(TowerType type)
    {
        AudioSource temp = GetTowerAudioSource();
        switch (type)
        {
            case TowerType.BASIC:
                temp.clip = towerShootBasic;
                temp.Play();
                break;
            case TowerType.RAPID:
                temp.clip = towerShootRapid;
                temp.Play();
                break;
        }
    }

}
