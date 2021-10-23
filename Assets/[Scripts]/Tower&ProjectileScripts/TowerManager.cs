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
