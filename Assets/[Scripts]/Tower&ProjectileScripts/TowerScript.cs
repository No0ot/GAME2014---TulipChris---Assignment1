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
    GameObject targetEnemy;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FindEnemy()
    {

    }

    private void Shoot()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            targetEnemy = collision.gameObject;
        }
    }
}
