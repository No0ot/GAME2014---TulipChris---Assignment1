using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    public int gold;
    public int iron;
    public int steel;
    public int totalKills;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gold = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
