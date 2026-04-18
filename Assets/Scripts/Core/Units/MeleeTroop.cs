using UnityEngine;

using System.Collections.Generic;

public class MeleeTroop : CombatUnit
{
    public int MaxTargets { get; private set; }

    private List<Enemy> targets = new List<Enemy>();

    public float RegenRate { get; private set; } = 5f;
    private float regenTimer;

    public MeleeTroop(int health, int damage, float attackCooldown, int maxTargets = 1)
    : base(health, damage, attackCooldown)
    {
        MaxTargets = maxTargets;
    }

    public bool CanTakeMoreTargets()
    {
        return targets.Count < MaxTargets;
    }

    public bool IsEngagedWith(Enemy enemy)
    {
        return targets.Contains(enemy);
    }

    public void AssignTarget(Enemy enemy)
    {
        if (!CanTakeMoreTargets()) return;
        if (targets.Contains(enemy)) return;

        targets.Add(enemy);
        IsInCombat = true;
    }

    public void RemoveTarget(Enemy enemy)
    {
        targets.Remove(enemy);

        if (targets.Count == 0)
        {
            IsInCombat = false;
        }
    }

    public void Tick(float deltaTime)
    {
        TickAttackTimer(deltaTime);

        if (!IsInCombat)
        {
            Regen(deltaTime);
        }

        TryAttack();
    }

    private void TryAttack()
    {
        if (!CanAttack() || targets.Count == 0)
            return;

        // Attack ALL current targets (your design choice)
        foreach (var enemy in targets)
        {
            if (enemy.IsAlive)
            {
                enemy.TakeDamage(Damage);
            }
        }

        ResetAttackTimer();
    }

    private void Regen(float deltaTime)
    {
        regenTimer += deltaTime;

        if (regenTimer >= RegenRate)
        {
            Health = System.Math.Min(MaxHealth, Health + 10);
            regenTimer = 0f;
        }
    }

    public override void OnDeath()
    {
        targets.Clear();
        IsInCombat = false;
    }
}