//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 19, 2021
//      File            : ProjectileFactory.cs
//      Description     : This script contains a factory pattern for instantiating different projectiles
//      History         :   v0.5 - Created the script with the create Projectile function.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [Header("Projectile Types")]
    public GameObject basicPrefab;
    public GameObject rapidPrefab;
    public GameObject misslePrefab;
    /// <summary>
    /// Creates a projectile of the passed in type and return a reference to it.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject CreateProjectile(ProjType type)
    {
        GameObject tempProj = null;
        switch (type)
        {
            case ProjType.BASIC:
                tempProj = Instantiate(basicPrefab);
                break;
            case ProjType.RAPID:
                tempProj = Instantiate(rapidPrefab);
                break;
            case ProjType.MISSLE:
                tempProj = Instantiate(misslePrefab);
                break;
        }
        tempProj.transform.parent = transform;
        return tempProj;
    }
}
