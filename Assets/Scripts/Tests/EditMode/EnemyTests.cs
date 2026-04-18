using UnityEngine;

using NUnit.Framework;

public class EnemyTests
{
    [Test]
    public void Enemy_TakesDamage_ReducesHealth()
    {
        var enemy = new Enemy(100);

        enemy.TakeDamage(30);

        Assert.AreEqual(70, enemy.Health);
    }

    [Test]
    public void Enemy_Dies_WhenHealthReachesZero()
    {
        var enemy = new Enemy(50);

        enemy.TakeDamage(50);

        Assert.IsFalse(enemy.IsAlive);
    }

    [Test]
    public void Enemy_Health_DoesNotGoBelowZero()
    {
        var enemy = new Enemy(50);

        enemy.TakeDamage(100);

        Assert.AreEqual(0, enemy.Health);
    }

    [Test]
    public void Enemy_CannotTakeDamage_WhenDead()
    {
        var enemy = new Enemy(50);

        enemy.TakeDamage(50); // kill
        enemy.TakeDamage(20); // extra hit

        Assert.AreEqual(0, enemy.Health);
    }
}