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
    public int damage;

    public float journeyLength;
    private float startTime;
    public Vector3 startPosition;
    public Vector3 endPosition;


    private void Update()
    {
        Move();
        if (targetEnemy)
        {
            if (!targetEnemy.activeSelf)
                targetEnemy = null;
        }
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
            collision.gameObject.GetComponent<EnemyScript>().currentHealth = collision.gameObject.GetComponent<EnemyScript>().currentHealth - damage;
            gameObject.SetActive(false);
        }
    }
}
