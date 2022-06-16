using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public delegate void OnBattleStartHandler();

    public static OnBattleStartHandler OnBattleStart;

    public static void RaiseOnBattleStart()
    {
        OnBattleStart?.Invoke();
    }

    public delegate void OnPlayerAttackHandler(float damage);
    public static OnPlayerAttackHandler OnPlayerAttack;
    public static void RaiseOnPlayerAttack(float d)
    {
        OnPlayerAttack?.Invoke(d);
    }

    public delegate void OnEnemyAttackHandler(float damage);
    public static OnEnemyAttackHandler OnEnemyAttack;
    public static void RaiseOnEnemyAttack(float damage)
    {
        OnEnemyAttack?.Invoke(damage);
    }
}
