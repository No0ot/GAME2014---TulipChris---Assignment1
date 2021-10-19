using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
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
        GameObject newprojectile = factory.CreateProjectile(projType);
        newprojectile.transform.SetParent(transform);
        newprojectile.SetActive(false);
        projectileList.Add(newprojectile);

        return newprojectile;
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
