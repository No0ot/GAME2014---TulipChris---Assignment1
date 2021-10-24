//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : ProjectileScript.cs
//      Description     : This script contains behaviours for the projectile objects.
//      History         :   v0.5 - Created the script along with the initial functions and enum used for the functionality.
//                          v0.7 - Added Rotate() method.
//                          v0.8 - Changed projectiles to only deal damage to targetEnemy even if it collides with multiple enemies at the same time.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjType
{
    BASIC,
    RAPID,
    MISSLE
}

public class ProjectileScript : MonoBehaviour
{
    public GameObject targetEnemy = null;
    public float moveSpeed;
    public float damage;

    private float journeyLength;
    private float startTime;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public TowerScript towerOwner;


    private void Update()
    {
        if (targetEnemy)
        {
            if (!targetEnemy.activeSelf)
                targetEnemy = null;
        }
        Move();
        Rotate();
    }
    private void OnEnable()
    {
        startPosition = transform.position;
        startTime = Time.time;
        
    }

    private void OnDisable()
    {
        targetEnemy = null;
    }
    /// <summary>
    /// Move function used to move the projectile to the target. Has Functionality that if the target is set to null it will move to the last known position of the target.
    /// </summary>
    private void Move()
    {
        if (targetEnemy)
            endPosition = targetEnemy.transform.position;

        journeyLength = Vector3.Distance(transform.position, endPosition);
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

        if(!targetEnemy)
        {
            float distance = Vector3.Distance(transform.position, endPosition);
            if (distance < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Trigger collision detection to detect if the enemy has been hit by the projectile, passes in its damage value to the enemy and then check its health to see if a kill needs to be recorded.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if (collision.gameObject == targetEnemy)
            {
                EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
                enemy.TakeDamage(damage);
                if (!enemy.CheckHealth())
                {
                    towerOwner.kills++;
                    PlayerStats.Instance.totalKills++;
                    enemy.AddReward();
                }
                gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Rotate method so the projectile is facing the way it is moving.
    /// </summary>
    private void Rotate()
    {
        Vector3 direction = new Vector3(transform.position.x - endPosition.x,
                                        transform.position.y - endPosition.y,
                                        0.0f);

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}
