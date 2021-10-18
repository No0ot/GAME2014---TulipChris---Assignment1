using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public TDGrid gameplayGrid;

    List<GameObject> enemyList;

    public bool spawnEnemy = false;

    private EnemyFactory factory;

    public EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<GameObject>();
        factory = GetComponent<EnemyFactory>();
    }

   private GameObject AddEnemy()
   {
       GameObject newEnemy = factory.CreateEnemy(enemyType);
       newEnemy.transform.SetParent(transform);
        enemyList.Add(newEnemy);
       
       return newEnemy;
   }

   public GameObject GetEnemy()
   {
       foreach (GameObject enemy in enemyList)
       {
           if (!enemy.activeInHierarchy)
           {
               return enemy;
           }
       }
       return AddEnemy();
   }

    public void SpawnEnemy()
    {
        if (spawnEnemy)
        {
            GameObject temp = GetEnemy();
            temp.transform.position = gameplayGrid.startTile.transform.position;
            temp.GetComponent<EnemyScript>().currentTile = gameplayGrid.startTile;
            temp.GetComponent<EnemyScript>().targetTile = gameplayGrid.startTile.pathNext;
            temp.SetActive(true);
            spawnEnemy = false;
        }
    }

    private void Update()
    {
        SpawnEnemy();
    }
}