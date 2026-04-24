using UnityEngine;

using NUnit.Framework;
using System.Collections.Generic;

public class TargetingTests
{
    [Test]
    public void Selects_FurthestEnemy()
    {
        var enemy1 = new Enemy(100) { PathProgress = 0.2f };
        var enemy2 = new Enemy(100) { PathProgress = 0.8f };
        var enemy3 = new Enemy(100) { PathProgress = 0.5f };

        var enemies = new List<Enemy> { enemy1, enemy2, enemy3 };

        var targeting = new FurthestAlongPathTargeting();

        var target = targeting.SelectTarget(enemies);

        Assert.AreEqual(enemy2, target);
    }

    [Test]
    public void Ignores_DeadEnemies()
    {
        var enemy1 = new Enemy(100) { PathProgress = 0.2f };
        var enemy2 = new Enemy(100) { PathProgress = 0.9f };

        enemy2.TakeDamage(999); // kill it

        var enemies = new List<Enemy> { enemy1, enemy2 };

        var targeting = new FurthestAlongPathTargeting();

        var target = targeting.SelectTarget(enemies);

        Assert.AreEqual(enemy1, target);
    }

    [Test]
    public void ReturnsNull_WhenNoEnemies()
    {
        var targeting = new FurthestAlongPathTargeting();

        var target = targeting.SelectTarget(new List<Enemy>());

        Assert.IsNull(target);
    }
}