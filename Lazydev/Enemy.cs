using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemySO data;

    public Image hpBar;

    public float CurrentHealth { get; set; }

    private void Awake()
    {
        BattleEvents.OnPlayerAttack += TakeDamage;
    }

    private void OnDestroy()
    {
        BattleEvents.OnPlayerAttack -= TakeDamage;
    }

    void Start()
    {
        CurrentHealth = data.maxHealth;
        hpBar.fillAmount = CurrentHealth / data.maxHealth;

        GameObject gfx = Instantiate(data.gfxPrefab, transform);
    }


    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        hpBar.fillAmount = CurrentHealth / data.maxHealth;
        Debug.Log("Enemy health is now " + CurrentHealth);
        if(CurrentHealth < 0)
        {
            Debug.Log("Enemy died, enter enemy death logic here!");
        }
        else
        {
            DealDamage();
        }
    }

    public void DealDamage()
    {
        BattleEvents.RaiseOnEnemyAttack(data.damage);
    }
}
