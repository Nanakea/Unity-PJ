using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10;

    public float CurrentHealth { get; set; }

    public Image healthBar;

    private void Awake()
    {
        BattleEvents.OnBattleStart += BattleStart;
        BattleEvents.OnEnemyAttack += TakeDamae;
    }
    private void OnDestroy()
    {
        BattleEvents.OnBattleStart += BattleStart;
        BattleEvents.OnEnemyAttack += TakeDamae;

    }

    private void TakeDamae(float damage)
    {
        CurrentHealth -= damage;

        healthBar.fillAmount = CurrentHealth / maxHealth;
        
    }

    private void BattleStart()
    {

    }

    void Start()
    {
        CurrentHealth = maxHealth;
    }

    void Update()
    {
        
    }
}
