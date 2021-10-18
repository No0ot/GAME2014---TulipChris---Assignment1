using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyType type;
    public Tile targetTile;
    public Tile currentTile;

    public int health;
    public bool armored;
    public float moveSpeed;

    private float journeyLength;
    private float startTime;
    private bool tileReached;

    private void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        Move();
    }

    private void Move()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(currentTile.transform.position, targetTile.transform.position, fractionOfJourney);
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, targetTile.transform.position);
        if (distance  < 0.01f)
        {
            if (targetTile.pathfindingState == PathfindingState.END)
            {
                gameObject.SetActive(false);
            }
            else
            {
                currentTile = targetTile;
                targetTile = targetTile.pathNext;
                startTime = Time.time;
                journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
            }
        }
    }
}
