//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 21, 2021
//      File            : PlayerStats.cs
//      Description     : Singleton containing player related variables such as resources and totalkills
//      History         :   v0.5 - Created the class
//                          v0.7 - Added a reference to the EnemyWaveSpawner to be easily passed into the GameManager
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    public int gold;
    public int iron;
    public int steel;
    public int totalKills;

    public EnemyWaveSpawner enemySpawner;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gold = 60;
    }
}
