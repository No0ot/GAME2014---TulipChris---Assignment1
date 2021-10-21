using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyType type;
    public Tile targetTile = null;
    public Tile currentTile = null;

    public int maxHealth;
    public int currentHealth;
    public bool armored;
    public float moveSpeed;

    private float journeyLength;
    private float startTime;

    int goldReward;
    int ironReward;
    int steelReward;
    public int goldMin;
    public int goldMax;
    public int ironMin;
    public int ironMax;
    public int steelMin;
    public int steelMax;

    private void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentHealth = maxHealth;
        goldReward = Random.Range(goldMin, goldMax);
        ironReward = Random.Range(ironMin, ironMax);
        steelReward = Random.Range(steelMin, steelMax);
    }
    private void OnEnable()
    {
        startTime = Time.time;
        //journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentHealth = maxHealth;
        goldReward = Random.Range(goldMin, goldMax);
        ironReward = Random.Range(ironMin, ironMax);
        steelReward = Random.Range(steelMin, steelMax);
    }
    // Update is called once per frame
    void Update()
    {
        //CheckHealth();
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
        if (distance < 0.01f)
        {
            if (targetTile.pathfindingState == PathfindingState.END)
            {
                GameManager.Instance.lives--;
                GameplayUIManager.Instance.UpdateLifeSprites();
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

    public bool CheckHealth()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }

    public void AddReward()
    {
        PlayerStats.Instance.gold += goldReward;
        PlayerStats.Instance.iron += ironReward;
        PlayerStats.Instance.gold += steelReward;
    }
}
