//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 18, 2021
//      File            : ProjectileManager.cs
//      Description     : This script contains methods used for the different projectile managers.
//      History         :   v0.5 - Created the script along with the initial functions used for the functionality.
//
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
    /// <summary>
    /// Access the factory to create a projectile and add it to the list.
    /// </summary>
    /// <returns></returns>
    private GameObject AddProjectile()
    {
        GameObject newprojectile = factory.CreateProjectile(projType);
        newprojectile.transform.SetParent(transform);
        newprojectile.SetActive(false);
        projectileList.Add(newprojectile);

        return newprojectile;
    }
    /// <summary>
    /// Returns a reference to an available projectile, if non available add a new one and return that.
    /// </summary>
    /// <returns></returns>
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
