//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : TowerScript.cs
//      Description     : This script contains behaviours for the tower objects.
//      History         :   v0.5 - Created the script along with the initial functions and enum used for the functionality (FindEnemy and Shoot methods along with trigger methods).
//                          v0.7 - Added Rotate() and upgrade tower Methods
//                          v0.9 - Added an additional check to see if the target enemy is out of range as it wasn't always working with the triggers.
//
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
    public List<GameObject> enemysInRange;
    public ProjectileManager projectileManager;

    CircleCollider2D rangeCollider;

    private void Start()
    {
        targetEnemy = null;
        enemysInRange = new List<GameObject>();
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.radius = range;
    }

    private void Update()
    {
        if(fireRateCounter <= fireRate)
            fireRateCounter += Time.deltaTime;

        CheckTargetRange();
        if (!targetEnemy)
            FindEnemy();
        else
            Shoot();
        Rotate();
    }
    /// <summary>
    /// Initially used to check distance between all enemies, now just returns the first in the list
    /// </summary>
    private void FindEnemy()
    {
        if(enemysInRange.Count > 0)
        {
            targetEnemy = enemysInRange[0];
        }
    }
    /// <summary>
    /// Gets a projectile from the corresponding manager and sets its target,damage and makes it active.
    /// </summary>
    private void Shoot()
    {
        if (fireRateCounter >= fireRate)
        {
            SoundManager.Instance.PlayTowerShoot(type);
            GameObject tempProj = projectileManager.GetProjectile();
            tempProj.transform.position = transform.position;
            tempProj.GetComponent<ProjectileScript>().targetEnemy = targetEnemy;
            tempProj.GetComponent<ProjectileScript>().damage = damage * level;
            tempProj.GetComponent<ProjectileScript>().towerOwner = this;
            tempProj.SetActive(true);
            fireRateCounter = 0f;
        }
    }
    /// <summary>
    /// Gizmos to show range in editor
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);

    }
    /// <summary>
    /// If an enemy enters the collider(range) adds it to the list.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemysInRange.Add(collision.gameObject);
            //Debug.Log("enter");
        }
    }
    /// <summary>
    /// If an enemy exits the collider(range) removes it from the list and if the enemy is the target enemy, set target enemy to null
    /// </summary>
    /// <param name="collision"></param>
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
    /// <summary>
    /// Rotates the turret to face the target enemy.
    /// </summary>
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
    /// <summary>
    /// Method used to update the tower and its stats when upgraded.
    /// </summary>
    public void UpgradeTower()
    {
        if(level < 3)
        {
            SoundManager.Instance.PlayTowerUpgrade();
            level++;
            if(level == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                PlayerStats.Instance.iron -= ironCost;
                fireRate *= 0.8f;
                range *= 1.1f;
                rangeCollider.radius = range;
            }
            else if(level == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                PlayerStats.Instance.steel -= steelCost;
                fireRate *= 0.8f;
                range *= 1.1f;
                rangeCollider.radius = range;
            }
        }
    }
    /// <summary>
    /// Additonal check on the target enemy to see if its in range, Did this because the triggers didnt always work.
    /// </summary>
    public void CheckTargetRange()
    {
        if(targetEnemy)
        {
            float distanceT = Vector3.Distance(transform.position, targetEnemy.transform.position);

            if(distanceT > range + 0.5)
            {
                enemysInRange.Remove(targetEnemy.gameObject);
                targetEnemy = null;
            }
        }
    }
}
