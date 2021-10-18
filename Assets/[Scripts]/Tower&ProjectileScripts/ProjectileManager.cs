using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public TDGrid gameplayGrid;

    List<GameObject> projectileList;

    private ProjectileFactory factory;

    public ProjType projType;

    // Start is called before the first frame update
    void Start()
    {
        projectileList = new List<GameObject>();
        factory = GetComponent<ProjectileFactory>();
    }

    private GameObject AddProjectile()
    {
        GameObject newEnemy = factory.CreateProjectile(projType);
        newEnemy.transform.SetParent(transform);
        projectileList.Add(newEnemy);

        return newEnemy;
    }

    public GameObject GetProjectile()
    {
        foreach (GameObject proj in projectileList)
        {
            if (!proj.activeInHierarchy)
            {
                return proj;
            }
        }
        return AddProjectile();
    }
}
