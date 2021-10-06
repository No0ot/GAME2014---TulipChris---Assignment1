using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    BASIC,
    ARMORED,
    FAST,
    TANK
}

public class EnemyScript : MonoBehaviour
{
    public EnemyType type;
    public Tile targetTile;

    public int health;
    public bool armored;
    public float moveSpeed;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Tile"))
        {
            if (collision.GetComponent<Tile>().pathfindingState == PathfindingState.END)
            {
                //subtract life, de-activate unit
            }
            else
                targetTile = collision.GetComponent<Tile>().pathNext;
        }
    }
}
