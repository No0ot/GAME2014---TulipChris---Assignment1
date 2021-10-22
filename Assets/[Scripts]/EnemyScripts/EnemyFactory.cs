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

    public float basicHealthIncrement = 1.0f;
    public float armoredHealthIncrement = 1.0f;
    public float fastHealthIncrement = 1.0f;
    public float tankHealthIncrement = 1.0f;

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
