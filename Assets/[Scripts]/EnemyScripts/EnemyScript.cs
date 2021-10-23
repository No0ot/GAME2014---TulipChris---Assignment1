using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyType type;
    public Tile targetTile = null;
    public Tile currentTile = null;

    public float maxHealth;
    public float currentHealth;
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

    public GameObject greenLifeBar;
    public GameObject lifeBar;
    Quaternion lifeBarRot;
    Vector3 lifeBarPos;

    private void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentHealth = maxHealth;
        goldReward = Random.Range(goldMin, goldMax);
        ironReward = Random.Range(ironMin, ironMax); 
        steelReward = Random.Range(steelMin, steelMax);
        lifeBarRot = lifeBar.transform.rotation;
        lifeBarPos = lifeBar.transform.position;
    }
    private void OnEnable()
    {
        startTime = Time.time;
        //journeyLength = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentHealth = maxHealth;
        goldReward = Random.Range(goldMin, goldMax);
        ironReward = Random.Range(ironMin, ironMax);
        steelReward = Random.Range(steelMin, steelMax);
        greenLifeBar.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        UpdateLifeBar();
        //audioSource.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        Move();
        Rotate();
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
                GameManager.Instance.CheckLives();
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
    public void TakeDamage(float damage)
    {
        if (!armored)
            currentHealth -= damage;
        else
            currentHealth -= damage / 2;
    }
    public bool CheckHealth()
    {
        if (currentHealth <= 0)
        {
            SoundManager.Instance.PlayRandomEnemyDeathSound();
            gameObject.SetActive(false);
            
            return false;
        }
        UpdateLifeBar();
        return true;
    }

    public void AddReward()
    {
        PlayerStats.Instance.gold += goldReward;
        PlayerStats.Instance.iron += ironReward;
        PlayerStats.Instance.gold += steelReward;
    }

    void UpdateLifeBar()
    {
        float temp = (float)currentHealth / (float)maxHealth;
        greenLifeBar.transform.localScale = new Vector3(1.0f * temp, greenLifeBar.transform.localScale.y, greenLifeBar.transform.localScale.z);
    }

    void Rotate()
    {
        Vector3 direction = new Vector3(targetTile.transform.position.x - currentTile.transform.position.x,
                                        targetTile.transform.position.y - currentTile.transform.position.y,
                                        0.0f);

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        lifeBar.transform.rotation = lifeBarRot;
        lifeBar.transform.position = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
    }
}
