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
    float armoredTimer = 0f;
    float fastTimer = 0f;
    float tankTimer = 0f;

    float timeBtwnBasic = 2.5f;
    float timeBtwnArmored = 5f;
    float timeBtwnFast = 2f;
    float timeBtwnTank = 30f;

    public bool basicActive = true;
    public bool armoredActive;
    public bool fastActive;
    public bool tankActive;

    private void Update()
    {
        SpawnBasic();
        SpawnArmored();
        SpawnFast();
        SpawnTank();
    }

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
}
