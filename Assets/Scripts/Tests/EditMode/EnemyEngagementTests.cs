using NUnit.Framework;
using System.Collections.Generic;

public class EnemyEngagementTests
{
    [Test]
    public void Enemy_EngagesAvailableTroop()
    {
        var enemy = new Enemy(100);
        var troop = new MeleeTroop(100, 10, 1f, 1);

        enemy.TryEngage(new List<MeleeTroop> { troop });

        Assert.AreEqual(Enemy.EnemyState.InCombat, enemy.State);
        Assert.IsTrue(troop.IsEngagedWith(enemy));
    }

    [Test]
    public void Enemy_DoesNotEngage_WhenNoTroopsAvailable()
    {
        var enemy = new Enemy(100);
        var troop = new MeleeTroop(100, 10, 1f, 0); // full

        enemy.TryEngage(new List<MeleeTroop> { troop });

        Assert.AreEqual(Enemy.EnemyState.Moving, enemy.State);
    }

    [Test]
    public void Enemy_Disengages_OnDeath()
    {
        var enemy = new Enemy(100);
        var troop = new MeleeTroop(100, 10, 1f, 1);

        enemy.TryEngage(new List<MeleeTroop> { troop });
        enemy.TakeDamage(999);

        Assert.AreEqual(Enemy.EnemyState.Dead, enemy.State);
        Assert.IsFalse(troop.IsEngagedWith(enemy));
    }
}