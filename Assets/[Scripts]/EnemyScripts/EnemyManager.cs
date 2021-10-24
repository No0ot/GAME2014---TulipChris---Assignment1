//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 23, 2021
//      File            : EnemyManager.cs
//      Description     : This script contains the list of enemies for the specific type used. Gets/spawns/increments health of the enemys in the list. 
//      History         :   v0.5 - Created the Manager class that accesses the attached factory to instatiate enemies and spawn them into the game.
//                          v0.55 - Added Enemy Type to set the type of enemy the manager will handle
//                          v0.7 - Added Increment health method.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public TDGrid gameplayGrid;

    List<GameObject> enemyList;

    //public bool spawnEnemy = false;

    private EnemyFactory factory;

    public EnemyType enemyType;

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<GameObject>();
        factory = GetComponent<EnemyFactory>();
    }
    /// <summary>
    /// Accesses factory to create a new enemy.
    /// </summary>
    /// <returns></returns>
   private GameObject AddEnemy()
   {
       GameObject newEnemy = factory.CreateEnemy(enemyType);
       newEnemy.transform.SetParent(transform);
       newEnemy.SetActive(false);
       enemyList.Add(newEnemy);
       
       return newEnemy;
   }
    /// <summary>
    /// Returns a reference to the first available enemy in the list or adds a new one if non available
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Spawns enemy into the gameplay grid and sets there active state to true
    /// </summary>
    public void SpawnEnemy()
    {
        if (gameplayGrid.endTile)
        {
            GameObject temp = GetEnemy();
            temp.transform.position = gameplayGrid.startTile.transform.position;
            temp.GetComponent<EnemyScript>().currentTile = gameplayGrid.startTile;
            temp.GetComponent<EnemyScript>().targetTile = gameplayGrid.startTile.pathNext;
            temp.SetActive(true);
        }
        //spawnEnemy = false;
    }
    /// <summary>
    /// Increments the health of enemies when called.
    /// </summary>
    public void IncrementEnemyHealth()
    {
        foreach(GameObject enemy in enemyList)
        {
            enemy.GetComponent<EnemyScript>().maxHealth *= 1.2f;
        }
        factory.IncrementHealth(enemyType);
    }
}
