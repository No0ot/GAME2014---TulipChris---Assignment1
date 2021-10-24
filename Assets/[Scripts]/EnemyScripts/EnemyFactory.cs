//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 22, 2021
//      File            : EnemyFactory.cs
//      Description     : This script contains the methods used to instantiate different enemys along with the method to Increment health of any enemys to be instantiated.
//      History         :   v0.5 - Added Create Enemy method.
//                          v0.7 - Added Increment health method.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Header("Enemy Types")]
    public GameObject basicEnemyPrefab;
    public GameObject armoredEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    float basicHealthIncrement = 1.0f;
    float armoredHealthIncrement = 1.0f;
    float fastHealthIncrement = 1.0f;
    float tankHealthIncrement = 1.0f;
    /// <summary>
    /// Creates an enemy based off of the passed in enemytype
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject CreateEnemy(EnemyType type)
    {
        GameObject tempEnemy = null;
        switch (type)
        {
            case EnemyType.BASIC:
                tempEnemy = Instantiate(basicEnemyPrefab);
                tempEnemy.GetComponent<EnemyScript>().maxHealth *= basicHealthIncrement;
                break;
            case EnemyType.ARMORED:
                tempEnemy = Instantiate(armoredEnemyPrefab);
                tempEnemy.GetComponent<EnemyScript>().maxHealth *= armoredHealthIncrement;
                break;
            case EnemyType.FAST:
                tempEnemy = Instantiate(fastEnemyPrefab);
                tempEnemy.GetComponent<EnemyScript>().maxHealth *= fastHealthIncrement;
                break;
            case EnemyType.TANK:
                tempEnemy = Instantiate(tankEnemyPrefab);
                tempEnemy.GetComponent<EnemyScript>().maxHealth *= tankHealthIncrement;
                break;
        }
        tempEnemy.transform.parent = transform;
        tempEnemy.SetActive(false);
        return tempEnemy;
    }
    /// <summary>
    /// Sets the healthincrement to be used when instantiating the enemys.
    /// </summary>
    /// <param name="type"></param>
    public void IncrementHealth(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.BASIC:
                basicHealthIncrement *= 1.2f;
                break;
            case EnemyType.ARMORED:
                armoredHealthIncrement *= 1.2f;
                break;
            case EnemyType.FAST:
                fastHealthIncrement *= 1.2f;
                break;
            case EnemyType.TANK:
                tankHealthIncrement *= 1.2f;
                break;
        }
    }
}
