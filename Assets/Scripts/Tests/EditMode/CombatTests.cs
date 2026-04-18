using UnityEngine;

using NUnit.Framework;

public class CombatTests
{
    [Test]
    public void Unit_TakesDamage_Correctly()
    {
        var enemy = new Enemy(100);

        enemy.TakeDamage(30);

        Assert.AreEqual(70, enemy.Health);
    }

    [Test]
    public void Unit_Dies_WhenHealthZero()
    {
        var enemy = new Enemy(50);

        enemy.TakeDamage(50);

        Assert.IsFalse(enemy.IsAlive);
    }

    [Test]
    public void AttackCooldown_Works()
    {
        var enemy = new Enemy(100);

        Assert.IsTrue(enemy.CanAttack());

        enemy.ResetAttackTimer();

        Assert.IsFalse(enemy.CanAttack());
    }

    [Test]
    public void AttackTimer_CountsDown()
    {
        var enemy = new Enemy(100);

        enemy.ResetAttackTimer();
        enemy.TickAttackTimer(1f);

        Assert.IsTrue(enemy.CanAttack());
    }
}