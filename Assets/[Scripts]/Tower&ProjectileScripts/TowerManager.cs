using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public ProjectileManager basicProjManager;
    public ProjectileManager rapidProjManager;
    public ProjectileManager missleProjManager;

    List<GameObject> towerList;
    private TowerFactory factory;


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


        return newTower;
    }

    public GameObject GetTower(TowerType type)
    {
        foreach (GameObject tower in towerList)
        {
            if (!tower.activeInHierarchy)
            {
                return tower;
            }
        }
        return AddTower(type);
    }
}
