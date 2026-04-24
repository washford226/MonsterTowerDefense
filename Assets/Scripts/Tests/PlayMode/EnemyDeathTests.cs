using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;

public class EnemyDeathTests
{
    [UnityTest]
    public IEnumerator Enemy_Dies_And_Triggers_Event()
    {
        GameObject go = new GameObject();
        EnemyView view = go.AddComponent<EnemyView>();

        Enemy enemy = new Enemy(10);
        view.Initialize(enemy);

        bool died = false;

        // 🔥 Subscribe to logic-layer event instead
        enemy.OnDeathEvent += (e) =>
        {
            died = true;
        };

        view.TakeDamage(999);

        yield return null;

        Assert.IsTrue(died);
        Assert.IsFalse(enemy.IsAlive);

        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator WaveManager_Spawns_Enemies_Over_Time()
    {
        var waves = new List<Wave>
    {
        new Wave(3, 0.1f)
    };

        var manager = new WaveManager(waves);

        int spawned = 0;

        for (int i = 0; i < 10; i++)
        {
            var enemy = manager.Tick(0.1f);

            if (enemy != null)
                spawned++;

            yield return null;
        }

        Assert.AreEqual(3, spawned);
    }

    [Test]
    public void Enemy_Should_Enter_Dead_State_When_Killed()
    {
        Enemy enemy = new Enemy(10);

        enemy.TakeDamage(999);

        Assert.IsFalse(enemy.IsAlive);
        Assert.AreEqual(Enemy.EnemyState.Dead, enemy.State);
    }

    [Test]
    public void Enemy_Should_Be_Removable_From_Simulation()
    {
        var sim = new CombatSimulation();

        Enemy enemy = new Enemy(10);

        sim.Enemies.Add(enemy);

        enemy.TakeDamage(999);

        sim.Enemies.Remove(enemy);

        Assert.IsFalse(sim.Enemies.Contains(enemy));
    }
}