using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : CombatUnit
{
    public enum EnemyState
    {
        Moving,
        InCombat,
        Dead
    }

    public EnemyState State { get; private set; } = EnemyState.Moving;

    public float EngageRange { get; private set; } = 1.5f;

    public MeleeTroop CurrentTargetTroop { get; private set; }

    // 🔥 Unity-side death event
    public event Action<Enemy> OnDeathEvent;

    public Enemy(int health, int damage = 5, float attackCooldown = 1f)
        : base(health, damage, attackCooldown)
    {
    }

    public float PathProgress { get; set; }

    // ✅ REQUIRED BY CombatUnit (fixes your error)
    public override void OnDeath()
    {
        // Logic-layer cleanup only (NO Unity calls here)

        State = EnemyState.Dead;

        if (CurrentTargetTroop != null)
        {
            CurrentTargetTroop.RemoveTarget(this);
            CurrentTargetTroop = null;
        }

        OnDeathEvent?.Invoke(this);
    }

    public void TryEngage(
    List<MeleeTroop> troops,
    Dictionary<MeleeTroop, TroopView> troopMap,
    Vector3 enemyPosition
)
    {
        if (State != EnemyState.Moving || !IsAlive)
            return;

        foreach (var troop in troops)
        {
            if (!troop.CanTakeMoreTargets())
                continue;

            if (!troopMap.TryGetValue(troop, out TroopView troopView))
                continue;

            float dist = Vector3.Distance(
                enemyPosition,
                troopView.transform.position
            );

            if (dist <= EngageRange)
            {
                Engage(troop);
                return;
            }
        }
    }

    private void Engage(MeleeTroop troop)
    {
        CurrentTargetTroop = troop;
        State = EnemyState.InCombat;

        troop.AssignTarget(this);
    }

    public void Disengage()
    {
        if (CurrentTargetTroop != null)
        {
            CurrentTargetTroop.RemoveTarget(this);
        }

        CurrentTargetTroop = null;
        State = EnemyState.Moving;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

    public void CheckCombatDistance(
    Dictionary<MeleeTroop, TroopView> troopMap,
    Vector3 enemyPosition
)
    {
        if (CurrentTargetTroop == null)
            return;

        if (!troopMap.TryGetValue(CurrentTargetTroop, out TroopView troopView))
        {
            Disengage();
            return;
        }

        float dist = Vector3.Distance(
            enemyPosition,
            troopView.transform.position
        );

        if (dist > EngageRange + 0.5f)
        {
            Disengage();
        }
    }
}