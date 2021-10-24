//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 23, 2021
//      File            : EnemyWaveSpawner.cs
//      Description     : This script contains the amount of time between when enemies will be spawned into the game. As well as when the difficulty should be increased
//      History         :   v0.5 - Created script and had functionality for all enemy types added.
//                          v0.9 - Added Difficulty increasing effects.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public EnemyManager basicManager;
    public EnemyManager armoredManager;
    public EnemyManager fastManager;
    public EnemyManager tankManager;

    float basicTimer = 0f;
    float armoredTimer = 5f;
    float fastTimer = 0f;
    float tankTimer = 30f;

    float timeBtwnBasic = 2.5f;
    float timeBtwnArmored = 5f;
    float timeBtwnFast = 2f;
    float timeBtwnTank = 30f;

    public bool basicActive = true;
    public bool armoredActive;
    public bool fastActive;
    public bool tankActive;

    private void Start()
    {
        timeBtwnBasic = 2.5f;
        timeBtwnArmored = 5f;
        timeBtwnFast = 2f;
        timeBtwnTank = 30f;

        armoredActive = false;
        fastActive = false;
        tankActive = false;
    }
    private void Update()
    {
        SpawnBasic();
        SpawnArmored();
        SpawnFast();
        SpawnTank();
    }
    /// <summary>
    /// Spawns a basic enemy on a timer.
    /// </summary>
    void SpawnBasic()
    {
        if(basicActive)
        {
            if (basicTimer >= timeBtwnBasic)
            {
                basicManager.SpawnEnemy();
                basicTimer = 0f;
            }
            else
                basicTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Spawns an armored enemy on a timer.
    /// </summary>
    void SpawnArmored()
    {
        if(armoredActive)
        {
            if (armoredTimer >= timeBtwnArmored)
            {
                armoredManager.SpawnEnemy();
                armoredTimer = 0f;
            }
            else
                armoredTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Spawns a fast enemy on a timer.
    /// </summary>
    void SpawnFast()
    {
        if(fastActive)
        {
            if (fastTimer >= timeBtwnFast)
            {
                fastManager.SpawnEnemy();
                fastTimer = 0f;
            }
            else
                fastTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Spawns a tank enemy on a timer.
    /// </summary>
    void SpawnTank()
    {
        if(tankActive)
        {
            if (tankTimer >= timeBtwnTank)
            {
                tankManager.SpawnEnemy();
                tankTimer = 0f;
            }
            else
                tankTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// If the enemy type is active in the spawning rotation, increments the health of that enemy type.
    /// </summary>
    public void IncrementHealth()
    {
        if (basicActive)
            basicManager.IncrementEnemyHealth();
        if (armoredActive)
            armoredManager.IncrementEnemyHealth();
        if (fastActive)
            fastManager.IncrementEnemyHealth();
        if (tankActive)
            tankManager.IncrementEnemyHealth();
    }
    /// <summary>
    /// If the enemy type is active in the spawning rotation, decrements the time between enemy spawns (to a minimum amount).
    /// </summary>
    public void DecrementTimer()
    {
        if (basicActive && timeBtwnBasic > 1f)
            timeBtwnBasic -= 0.25f;
        if (armoredActive && timeBtwnArmored > 1f)
            timeBtwnArmored -= 0.25f;
        if (fastActive && timeBtwnFast > 0.5f)
            timeBtwnFast -= 0.25f;
        if (tankActive && timeBtwnTank > 10.0f)
            timeBtwnTank -= 5f;
    }
}
