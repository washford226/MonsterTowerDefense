using UnityEngine;

public class Arrow
{
    public Enemy Target { get; private set; }
    public int Damage { get; private set; }

    private float travelTime;
    private float timer;
    private bool hasHit;

    public bool IsComplete => hasHit;

    public Arrow(Enemy target, int damage, float travelTime)
    {
        Target = target;
        Damage = damage;
        this.travelTime = travelTime;
        timer = travelTime;
    }

    public void Tick(float deltaTime)
    {
        if (hasHit) return;

        timer -= deltaTime;

        if (timer <= 0f)
        {
            Hit();
        }
    }

    private void Hit()
    {
        if (hasHit) return;

        hasHit = true;

        if (Target != null && Target.IsAlive)
        {
            Target.TakeDamage(Damage);
        }
    }
}