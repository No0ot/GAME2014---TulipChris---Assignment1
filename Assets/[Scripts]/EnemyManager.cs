using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public TDGrid gameplayGrid;

    public GameObject basicEnemyPrefab;
    public GameObject armoredEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    List<GameObject> basicEnemyList;
    List<GameObject> armoredEnemyList;
    List<GameObject> fastEnemyList;
    List<GameObject> tankEnemyList;

    int maxBasicEnemies = 20;
    int maxArmoredEnemies = 20;
    int maxFastEnemies = 20;
    int maxTankEnemies = 5;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateEnemy(basicEnemyPrefab, maxBasicEnemies, basicEnemyList);
        InstantiateEnemy(armoredEnemyPrefab, maxArmoredEnemies, armoredEnemyList);
        InstantiateEnemy(fastEnemyPrefab, maxFastEnemies, fastEnemyList);
        InstantiateEnemy(tankEnemyPrefab, maxTankEnemies, tankEnemyList);
    }

    void InstantiateEnemy(GameObject prefab, int maxnum, List<GameObject> list)
    {
        list = new List<GameObject>(maxnum);

        for (int i = 0; i < maxnum; i++)
        {
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.transform.SetParent(transform);
            newEnemy.SetActive(false);
            newEnemy.GetComponent<EnemyScript>().targetTile = gameplayGrid.startTile;
            list.Add(newEnemy);
        }
    }
}
