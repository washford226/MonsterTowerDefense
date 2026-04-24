using UnityEngine;

using System.Collections.Generic;

public class ArcherTower
{
    public System.Action<Enemy> OnShoot;
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

    public Enemy TryGetTarget(List<Enemy> enemies, Dictionary<Enemy, EnemyView> map, Vector3 towerPosition)
    {
        var inRange = new List<Enemy>();

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;

            if (!map.TryGetValue(enemy, out EnemyView view))
                continue;

            float dist = Vector3.Distance(towerPosition, view.transform.position);

            if (dist <= Range)
            {
                inRange.Add(enemy);
            }
        }

        return targetingStrategy.SelectTarget(inRange);
    }

    public Enemy Attack(List<Enemy> enemies, Dictionary<Enemy, EnemyView> map, Vector3 towerPosition)
    {
        if (!CanAttack())
            return null;

        var target = TryGetTarget(enemies, map, towerPosition);

        if (target == null)
            return null;

        attackTimer = AttackCooldown;

        OnShoot?.Invoke(target);

        return target;
    }
}