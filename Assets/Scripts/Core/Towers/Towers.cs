using UnityEngine;

using System.Collections.Generic;

public class ArcherTower
{
    public float Range { get; private set; }
    public int Damage { get; private set; }
    public float AttackCooldown { get; private set; }

    private float attackTimer;

    private ITargetingStrategy targetingStrategy;

    public ArcherTower(float range, int damage, float attackCooldown, ITargetingStrategy strategy)
    {
        Range = range;
        Damage = damage;
        AttackCooldown = attackCooldown;
        targetingStrategy = strategy;
    }

    public bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    public void Tick(float deltaTime)
    {
        if (attackTimer > 0)
            attackTimer -= deltaTime;
    }

    public Enemy TryGetTarget(List<Enemy> enemies)
    {
        var inRange = enemies.FindAll(e => e.IsAlive);

        return targetingStrategy.SelectTarget(inRange);
    }

    public Enemy Attack(List<Enemy> enemies)
    {
        if (!CanAttack())
            return null;

        var target = TryGetTarget(enemies);

        if (target == null)
            return null;

        target.TakeDamage(Damage);
        attackTimer = AttackCooldown;

        return target;
    }
}