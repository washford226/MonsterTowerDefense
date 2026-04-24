using UnityEngine;

using NUnit.Framework;

public class WaveTests
{
    [Test]
    public void Wave_SpawnsEnemiesOverTime()
    {
        var manager = new WaveManager(new System.Collections.Generic.List<Wave>
        {
            new Wave(3, 1f)
        });

        int spawned = 0;

        for (int i = 0; i < 10; i++)
        {
            var enemy = manager.Tick(1f);

            if (enemy != null)
                spawned++;
        }

        Assert.AreEqual(3, spawned);
    }

    [Test]
    public void Wave_CompletesAndStops()
    {
        var manager = new WaveManager(new System.Collections.Generic.List<Wave>
        {
            new Wave(2, 0f)
        });

        for (int i = 0; i < 5; i++)
        {
            manager.Tick(1f);
        }

        Assert.IsTrue(manager.IsFinished);
    }
}