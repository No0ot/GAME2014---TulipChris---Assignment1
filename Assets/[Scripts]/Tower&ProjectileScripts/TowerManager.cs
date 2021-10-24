//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 18, 2021
//      File            : TowerManager.cs
//      Description     : This script contains behaviours for the Tower managers.
//      History         :   v0.5 - Created the script along with the initial functions used for the functionality.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public ProjectileManager basicProjManager;
    public ProjectileManager rapidProjManager;
    public ProjectileManager missleProjManager;

    List<GameObject> towerList;
    public TowerFactory factory;


    // Start is called before the first frame update
    void Start()
    {
        towerList = new List<GameObject>();
        factory = GetComponent<TowerFactory>();
    }
    /// <summary>
    /// Ask the factory to instantiate a tower of the passed in type and add it to the list.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject AddTower(TowerType type)
    {
        GameObject newTower = factory.CreateTower(type);
        switch(type)
        {
            case TowerType.BASIC:
                newTower.GetComponent<TowerScript>().projectileManager = basicProjManager;
                break;
            case TowerType.RAPID:
                newTower.GetComponent<TowerScript>().projectileManager = rapidProjManager;
                break;
            case TowerType.MISSLE:
                newTower.GetComponent<TowerScript>().projectileManager = missleProjManager;
                break;
        }
        newTower.transform.SetParent(transform);
        towerList.Add(newTower);

        PlayerStats.Instance.gold -= newTower.GetComponent<TowerScript>().goldCost;
        return newTower;
    }
    /// <summary>
    /// Returns a reference to a tower, if one doesnt exist add a new one.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetTower(TowerType type)
    {
        foreach (GameObject tower in towerList)
        {
            if (!tower.activeInHierarchy)
            {
                PlayerStats.Instance.gold -= tower.GetComponent<TowerScript>().goldCost;
                return tower;
            }
        }
        return AddTower(type);
    }
}
