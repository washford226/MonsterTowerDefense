using NUnit.Framework;
using UnityEngine;

public class ArrowTests
{
    [Test]
    public void Arrow_DealsDamage_WhenItArrives()
    {
        var enemy = new Enemy(100);
        var arrow = new Arrow(enemy, 20, travelTime: 1f);

        arrow.Tick(1f);

        Assert.AreEqual(80, enemy.Health, "Arrow should deal damage on impact");
        Assert.IsTrue(arrow.IsComplete, "Arrow should complete after impact");
    }

    [Test]
    public void Arrow_DoesNothing_IfEnemyDiesBeforeImpact()
    {
        var enemy = new Enemy(20);
        var arrow = new Arrow(enemy, 50, 1f);

        enemy.TakeDamage(20); // enemy dies before arrow arrives

        arrow.Tick(1f);

        Assert.AreEqual(0, enemy.Health, "Enemy should remain dead");
        Assert.IsTrue(arrow.IsComplete, "Arrow should still complete even if target is dead");
    }

    [Test]
    public void Arrow_CannotDealDamageTwice()
    {
        var enemy = new Enemy(100);
        var arrow = new Arrow(enemy, 20, 1f);

        arrow.Tick(1f);

        int afterFirstHit = enemy.Health;

        arrow.Tick(1f); // extra tick

        int afterSecondTick = enemy.Health;

        Assert.AreEqual(80, afterFirstHit, "First impact should deal damage once");
        Assert.AreEqual(afterFirstHit, afterSecondTick, "Arrow must NOT deal damage twice");
    }

    [Test]
    public void Arrow_DoesNotCrash_WhenEnemyIsNullOrDead()
    {
        var enemy = new Enemy(100);
        var arrow = new Arrow(enemy, 20, 1f);

        enemy.TakeDamage(999); // force death

        arrow.Tick(1f);

        Assert.IsTrue(arrow.IsComplete, "Arrow should complete even if enemy is already dead");
    }

    [Test]
    public void Arrow_RespectsTravelTime_BeforeDealingDamage()
    {
        var enemy = new Enemy(100);
        var arrow = new Arrow(enemy, 20, 2f);

        arrow.Tick(1f);

        Assert.AreEqual(100, enemy.Health, "Arrow should NOT deal damage before travel time completes");

        arrow.Tick(1f);

        Assert.AreEqual(80, enemy.Health, "Arrow should deal damage after full travel time");
    }
}