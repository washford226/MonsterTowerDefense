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
        if (enemy == null || !enemy.IsAlive) return;
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
        CleanupDeadTargets(); // 🔥 important safety step
    }

    private void TryAttack()
    {
        if (!CanAttack() || targets.Count == 0)
            return;

        // 🔥 SAFE LOOP (no foreach)
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            Enemy enemy = targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                targets.RemoveAt(i);
                continue;
            }

            enemy.TakeDamage(Damage);
        }

        ResetAttackTimer();
    }

    private void CleanupDeadTargets()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            var enemy = targets[i];

            if (enemy == null || !enemy.IsAlive)
            {
                targets.RemoveAt(i);
            }
        }

        if (targets.Count == 0)
        {
            IsInCombat = false;
        }
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