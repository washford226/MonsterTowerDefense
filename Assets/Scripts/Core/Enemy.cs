using UnityEngine;
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

    public MeleeTroop CurrentTargetTroop { get; private set; }

    public Enemy(int health, int damage = 5, float attackCooldown = 1f)
        : base(health, damage, attackCooldown)
    {
    }

    public float PathProgress { get; set; }

    public override void OnDeath()
    {
        // Later: give gold, trigger wave system, etc.
    }

    public void TryEngage(List<MeleeTroop> troops)
    {
        if (State != EnemyState.Moving || !IsAlive)
            return;

        foreach (var troop in troops)
        {
            if (troop.CanTakeMoreTargets())
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

        if (!IsAlive)
        {
            State = EnemyState.Dead;

            if (CurrentTargetTroop != null)
            {
                CurrentTargetTroop.RemoveTarget(this);
            }
        }
    }
}