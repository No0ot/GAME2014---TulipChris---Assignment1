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
