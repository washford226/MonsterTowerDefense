using UnityEngine;
using System;

public class EnemyView : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int damage = 5;
    public float attackCooldown = 1f;
    public float moveSpeed = 2f;
    public float engageRange = 1.5f;

    public Enemy Data { get; private set; }

    public event Action<Enemy> OnDeath;

    private bool isDead = false;

    public Enemy CreateEnemy()
    {
        return new Enemy(
            maxHealth,
            damage,
            attackCooldown,
            engageRange
        );
    }

    public void Initialize(Enemy enemy)
    {
        Data = enemy;

        Data.OnDeathEvent += HandleDeath;
    }

    public void TakeDamage(int amount)
    {
        if (Data == null || isDead) return;

        Data.TakeDamage(amount);
    }

    void HandleDeath(Enemy enemy)
    {
        if (isDead) return;

        isDead = true;

        OnDeath?.Invoke(Data);

        Data = null;

        var ai = GetComponent<EnemyAI>();

        if (ai != null)
            ai.enabled = false;

        var col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        Destroy(gameObject);
    }
}