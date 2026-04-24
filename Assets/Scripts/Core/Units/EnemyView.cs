using UnityEngine;
using System;

public class EnemyView : MonoBehaviour
{
    public Enemy Data { get; private set; }

    public event Action<Enemy> OnDeath;

    private bool isDead = false;

    public void Initialize(Enemy enemy)
    {
        Data = enemy;

        // 🔥 NEW: subscribe to logic death event
        Data.OnDeathEvent += HandleDeath;
    }

    public void TakeDamage(int amount)
    {
        if (Data == null || isDead) return;

        Data.TakeDamage(amount);
    }

    // 🔥 Now ONLY called from Enemy logic event
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