using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [Header("Projectile Types")]
    public GameObject basicPrefab;
    public GameObject rapidPrefab;
    public GameObject misslePrefab;

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
