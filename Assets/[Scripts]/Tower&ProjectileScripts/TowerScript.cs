using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    BASIC,
    RAPID,
    QUAKE,
    MISSLE,
    NONE
}

public class TowerScript : MonoBehaviour
{
    public float fireRate;
    public float range;

    public GameObject projectile;
    public TowerType type;
   

    public GameObject targetEnemy;
    public ProjectileManager projectileManager;

    private void Start()
    {
        targetEnemy = null;

        switch(type)
        {
            case TowerType.BASIC:
               // projectileManager = 
                break;
            case TowerType.RAPID:
                break;
            case TowerType.MISSLE:
                break;
        }
    }

    private void Update()
    {
        if (!targetEnemy)
            FindEnemy();
        else
            Shoot();

    }

    private void FindEnemy()
    {
        EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();

        foreach(EnemyScript enemy in enemies)
        {
            if(targetEnemy)
            {
                float distanceT = Vector3.Distance(transform.gameObject.transform.position, targetEnemy.transform.position);
                float distanceE = Vector3.Distance(transform.gameObject.transform.position, enemy.transform.position);

                if(distanceE > distanceT)
                    targetEnemy = enemy.gameObject;
            }
            targetEnemy = enemy.gameObject;
        }
                
    }

    private void Shoot()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);

    }

    private void OnTrigger2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!targetEnemy)
                targetEnemy = collision.gameObject;
            //Debug.Log("enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject == targetEnemy)
                targetEnemy = null;
            
            Debug.Log("exit");
        }
    }
}
