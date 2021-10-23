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
    public int damage;

    public TowerType type;
    public int level = 1;
    public int kills = 0;
    
    public int goldCost;
    public int ironCost;
    public int steelCost;

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
        Rotate();
    }

    private void FindEnemy()
    {
        if(enemysInRange.Count > 0)
        {
            targetEnemy = enemysInRange[0];
        }
    }

    private void Shoot()
    {
        if (fireRateCounter >= fireRate)
        {
            GameObject tempProj = projectileManager.GetProjectile();
            tempProj.transform.position = transform.position;
            tempProj.GetComponent<ProjectileScript>().targetEnemy = targetEnemy;
            tempProj.GetComponent<ProjectileScript>().damage = damage * level;
            tempProj.GetComponent<ProjectileScript>().towerOwner = this;
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
            
            //Debug.Log("exit");
        }
    }

    void Rotate()
    {
        if (targetEnemy)
        {
            Vector3 direction = new Vector3(transform.position.x - targetEnemy.transform.position.x,
                                            transform.position.y - targetEnemy.transform.position.y,
                                            0.0f);
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }
        }
        else
            transform.Rotate(new Vector3(0.0f, 0.0f, 10.0f * Time.deltaTime), Space.World);
    }
}
