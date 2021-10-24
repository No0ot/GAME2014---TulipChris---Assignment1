//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 18, 2021
//      File            : TowerFactory.cs
//      Description     : This script contains the factory pattern for instantiating objects of different tower types.
//      History         :   v0.5 - Created the script along with the initial functions used for the functionality.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [Header("Tower Types")]
    public GameObject basicTowerPrefab;
    public GameObject rapidTowerPrefab;
    public GameObject quakeTowerPrefab;
    public GameObject missleTowerPrefab;

    List<GameObject> towerList;

    private void Start()
    {
        towerList = new List<GameObject>();
    }
    /// <summary>
    /// Creates a tower of the passed in type and returns a reference to it.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject CreateTower(TowerType type)
    {
        GameObject tempTower = null;
        switch (type)
        {
            case TowerType.BASIC:
                tempTower = Instantiate(basicTowerPrefab);
                break;
            case TowerType.RAPID:
                tempTower = Instantiate(rapidTowerPrefab);
                break;
            case TowerType.QUAKE:
                tempTower = Instantiate(quakeTowerPrefab);
                break;
            case TowerType.MISSLE:
                tempTower = Instantiate(missleTowerPrefab);
                break;
        }
        tempTower.transform.parent = transform;
        towerList.Add(tempTower);
        return tempTower;
    }
}
