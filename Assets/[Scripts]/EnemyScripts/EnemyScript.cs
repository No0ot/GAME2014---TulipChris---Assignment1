//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 23, 2021
//      File            : EnemyScript.cs
//      Description     : This script contains the behaviours to be used on the attached enemy.
//      History         :   v0.3 - Created script and had enemies follow path created for them
//                          v0.5 - Enemies now take damage and despawn on death.
//                          v0.7 - Added Rotate method(steering behaviour) and Life bar
//                          v0.9 - Added Sounds
//
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
    /// <summary>
    /// Moves the enemy from one tile to the next.
    /// </summary>
    private void Move()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(currentTile.transform.position, targetTile.transform.position, fractionOfJourney);
    }
    /// <summary>
    /// Checks the distance to the target tile to see if it needs to update the target tile or if it reaches the end of the path takes a life away.
    /// </summary>
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
    /// <summary>
    /// Method used to take damage.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (!armored)
            currentHealth -= damage;
        else
            currentHealth -= damage / 2;
    }
    /// <summary>
    /// Checks the health of the enemy to see if it needs to be "killed" and despawned
    /// </summary>
    /// <returns></returns>
    public bool CheckHealth()
    {
        if (currentHealth <= 0)
        {
            SoundManager.Instance.PlayRandomEnemyDeathSound();
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            return false;
        }
        UpdateLifeBar();
        return true;
    }
    /// <summary>
    /// Adds reward to the player if killed.
    /// </summary>
    public void AddReward()
    {
        PlayerStats.Instance.gold += goldReward;
        PlayerStats.Instance.iron += ironReward;
        PlayerStats.Instance.steel += steelReward;
    }
    /// <summary>
    /// Updates the Life bar of the enemy.
    /// </summary>
    void UpdateLifeBar()
    {
        float temp = (float)currentHealth / (float)maxHealth;
        greenLifeBar.transform.localScale = new Vector3(1.0f * temp, greenLifeBar.transform.localScale.y, greenLifeBar.transform.localScale.z);
    }
    /// <summary>
    /// rotates the enemy to face the direction they are currently moving. Also makes sure the life bar doesnt rotate with the enemy.
    /// </summary>
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
