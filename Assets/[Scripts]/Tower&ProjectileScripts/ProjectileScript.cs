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
