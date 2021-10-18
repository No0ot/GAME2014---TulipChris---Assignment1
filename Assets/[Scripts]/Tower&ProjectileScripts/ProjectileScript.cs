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
    GameObject targetEnemy;
    public float moveSpeed;
    public int damage;

    private float journeyLength;
    private float startTime;
    Vector3 startPosition;


    private void OnEnable()
    {
        startPosition = transform.position;
    }
    private void Move()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, targetEnemy.transform.position, fractionOfJourney);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Debug.Log("hit");
        }
    }
}
