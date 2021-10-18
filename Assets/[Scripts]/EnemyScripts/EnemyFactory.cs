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

    public GameObject CreateEnemy(EnemyType type)
    {
        GameObject tempEnemy = null;
        switch (type)
        {
            case EnemyType.BASIC:
                tempEnemy = Instantiate(basicEnemyPrefab);
                break;
            case EnemyType.ARMORED:
                tempEnemy = Instantiate(armoredEnemyPrefab);
                break;
            case EnemyType.FAST:
                tempEnemy = Instantiate(fastEnemyPrefab);
                break;
            case EnemyType.TANK:
                tempEnemy = Instantiate(tankEnemyPrefab);
                break;
        }
        tempEnemy.transform.parent = transform;
        tempEnemy.SetActive(false);
        return tempEnemy;
    }
}
