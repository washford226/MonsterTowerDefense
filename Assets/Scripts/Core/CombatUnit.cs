using UnityEngine;

public abstract class CombatUnit
{
    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }

    public bool IsAlive => Health > 0;

    public bool IsInCombat { get; protected set; }

    public float AttackCooldown { get; protected set; }
    protected float attackTimer;

    public int Damage { get; protected set; }

    public CombatUnit(int health, int damage, float attackCooldown)
    {
        MaxHealth = health;
        Health = health;
        Damage = damage;
        AttackCooldown = attackCooldown;
    }

    public virtual void TakeDamage(int amount)
    {
        if (!IsAlive) return;

        Health -= amount;

        if (Health <= 0)
        {
            Health = 0;
            OnDeath();
        }
    }

    public bool CanAttack()
    {
        return attackTimer <= 0f && IsAlive;
    }

    public void TickAttackTimer(float deltaTime)
    {
        if (attackTimer > 0)
            attackTimer -= deltaTime;
    }

    public void ResetAttackTimer()
    {
        attackTimer = AttackCooldown;
    }

    public abstract void OnDeath();
}