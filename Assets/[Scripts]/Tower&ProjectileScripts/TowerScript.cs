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
    public float fireRateCounter;
    public float range;

    public GameObject projectile;
    public TowerType type;
   

    public GameObject targetEnemy;
    List<GameObject> enemysInRange;
    public ProjectileManager projectileManager;

    private void Start()
    {
        targetEnemy = null;
        enemysInRange = new List<GameObject>();
    }

    private void Update()
    {
        if(fireRateCounter <= fireRate)
            fireRateCounter += Time.deltaTime;
        if (!targetEnemy)
            FindEnemy();
        else
            Shoot();
    }

    private void FindEnemy()
    {
        if(enemysInRange.Count > 0)
        {
            targetEnemy = enemysInRange[0];
        }
        //EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
        //
        //foreach(EnemyScript enemy in enemies)
        //{
        //    if(targetEnemy)
        //    {
        //        float distanceT = Vector3.Distance(transform.gameObject.transform.position, targetEnemy.transform.position);
        //        float distanceE = Vector3.Distance(transform.gameObject.transform.position, enemy.transform.position);
        //
        //        if(distanceE > distanceT)
        //            targetEnemy = enemy.gameObject;
        //    }
        //    //targetEnemy = enemy.gameObject;
        //}
                
    }

    private void Shoot()
    {
        if (fireRateCounter >= fireRate)
        {
            GameObject tempProj = projectileManager.GetProjectile();
            tempProj.transform.position = transform.position;
            tempProj.GetComponent<ProjectileScript>().targetEnemy = targetEnemy;
            tempProj.SetActive(true);
            fireRateCounter = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemysInRange.Add(collision.gameObject);
            //Debug.Log("enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject == targetEnemy)
                targetEnemy = null;
            enemysInRange.Remove(collision.gameObject);
            
            Debug.Log("exit");
        }
    }
}
