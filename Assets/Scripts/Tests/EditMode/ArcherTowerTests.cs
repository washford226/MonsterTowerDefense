using UnityEngine;

using NUnit.Framework;
using System.Collections.Generic;

public class ArcherTowerTests
{
    [Test]
    public void Tower_Attacks_FurthestEnemy()
    {
        var enemy1 = new Enemy(100) { PathProgress = 0.2f };
        var enemy2 = new Enemy(100) { PathProgress = 0.8f };

        var enemies = new List<Enemy> { enemy1, enemy2 };

        var tower = new ArcherTower(
            range: 10,
            damage: 20,
            attackCooldown: 1f,
            strategy: new FurthestAlongPathTargeting()
        );

        tower.Attack(enemies);

        Assert.AreEqual(100, enemy1.Health);
        Assert.AreEqual(80, enemy2.Health);
    }

    [Test]
    public void Tower_RespectsCooldown()
    {
        var enemy = new Enemy(100) { PathProgress = 0.5f };
        var enemies = new List<Enemy> { enemy };

        var tower = new ArcherTower(10, 20, 1f, new FurthestAlongPathTargeting());

        var first = tower.Attack(enemies);
        var second = tower.Attack(enemies);

        Assert.IsNotNull(first);
        Assert.IsNull(second);
    }

    [Test]
    public void Tower_DoesNothing_WhenNoEnemies()
    {
        var tower = new ArcherTower(10, 20, 1f, new FurthestAlongPathTargeting());

        var result = tower.Attack(new List<Enemy>());

        Assert.IsNull(result);
    }
}