//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : SoundManager.cs
//      Description     : Singleton containing all needed audio clips that can be accessed and played on attached audio sources.
//      History         :   v0.5 - Added References to all Audio Clips and created corresponding functions to access and play the sounds.
//                          v0.7 - Created "Manager" like functions to dynamically instantiate the needed number of audio Sources for enemy units and towers.
// 
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    //Mixer Group
    public AudioMixerGroup[] mixergroup;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        menuMusicSource.clip = menuMusic;
        menuMusicSource.Play();
        enemyAudioSourceList = new List<AudioSource>();
        towerAudioSourceList = new List<AudioSource>();
    }
    /// <summary>
    /// Switches between music tracks in gameplay scene.
    /// </summary>
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
    /// <summary>
    /// Dynamically creating and getting audio sources for enemy sounds sources. I did this so all enemies can make sounds at the same time.
    /// </summary>
    /// <returns></returns>
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
        temp.outputAudioMixerGroup = mixergroup[3];
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        enemyAudioSourceList.Add(temp);
        return temp;
    }
    /// <summary>
    /// Dynamically creating and getting audio sources for tower sounds.
    /// </summary>
    /// <returns></returns>
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
        temp.outputAudioMixerGroup = mixergroup[4];
        temp.playOnAwake = false;
        temp.volume = 0.1f;
        towerAudioSourceList.Add(temp);
        return temp;
    }
    /// <summary>
    /// Plays towerBuild sound.
    /// </summary>
    public void PlayTowerBuild()
    {
        uiSoundSource.clip = towerBuild;
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays Tower Upgrade Sound.
    /// </summary>
    public void PlayTowerUpgrade()
    {
        uiSoundSource.clip = towerUpgrade;
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays Main Menu/End Menu music
    /// </summary>
    public void PlayMenuMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = null;
        menuMusicSource.clip = menuMusic;
        menuMusicSource.Play();
    }
    /// <summary>
    /// Plays initial Gameplay music track
    /// </summary>
    public void PlayGameplayMusic()
    {
        menuMusicSource.Stop();
        bgMusicSource.Stop();
        bgMusicSource.clip = gameplayTrack1;
        bgMusicSource.Play();
    }
    /// <summary>
    /// Plays dig Sound.
    /// </summary>
    public void PlayDigSound()
    {
        uiSoundSource.clip = digSound;
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays error sound.
    /// </summary>
    public void PlayErrorSound()
    {
        uiSoundSource.clip = errorSound;
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays a random click sound for moving forward in menus.
    /// </summary>
    public void PlayRandomClickForward()
    {
        uiSoundSource.clip = clickForward[Random.Range(0, (clickForward.Length))];
        uiSoundSource.Play();
    }
    /// <summary>
    /// Plays a random click sound for moving backward in menus.
    /// </summary>
    public void PlayRandomClickBackward()
    {
        uiSoundSource.clip = clickBack[Random.Range(0, (clickBack.Length))];
        uiSoundSource.Play();
    }
    /// <summary>
    /// Gets an audio source and plays a random enemy death sound.
    /// </summary>
    public void PlayRandomEnemyDeathSound()
    {
        AudioSource temp = GetEnemyAudioSource();
        temp.clip = enemyDie[Random.Range(0, (enemyDie.Length))];
        temp.Play();
    }
    /// <summary>
    /// Plays a different tower shoot sound based on which tower is accessing the function.
    /// </summary>
    /// <param name="type"></param>
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
