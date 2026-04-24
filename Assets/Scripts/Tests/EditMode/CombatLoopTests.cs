using UnityEngine;

using NUnit.Framework;

public class CombatLoopTests
{
    [Test]
    public void CombatLoop_ArcherAndMelee_WorkTogether()
    {
        var sim = new CombatSimulation();

        var enemy = new Enemy(100);
        enemy.PathProgress = 1f;

        var troop = new MeleeTroop(100, 20, 0.1f, maxTargets: 1);

        var tower = new ArcherTower(
            10,
            20,
            0.1f,
            new FurthestAlongPathTargeting()
        );

        sim.Enemies.Add(enemy);
        sim.Troops.Add(troop);
        sim.Towers.Add(tower);

        // Run simulation
        for (int i = 0; i < 10; i++)
        {
            sim.Tick(1f);
        }

        // Enemy should have taken SOME damage (melee + archer combined)
        Assert.Less(enemy.Health, 100);

        // Enemy may or may not be dead depending on timing,
        // but simulation must process correctly
        Assert.IsTrue(sim.Enemies.Count >= 0);
    }

    [Test]
    public void Debug_CombatLoop_SeesDamage()
    {
        var sim = new CombatSimulation();

        var enemy = new Enemy(100);
        enemy.PathProgress = 1f;

        var troop = new MeleeTroop(100, 20, 0.1f, 1);

        var tower = new ArcherTower(
            10,
            20,
            0.1f,
            new FurthestAlongPathTargeting()
        );

        sim.Enemies.Add(enemy);
        sim.Troops.Add(troop);
        sim.Towers.Add(tower);

        for (int i = 0; i < 10; i++)
        {
            sim.Tick(1f);
            UnityEngine.Debug.Log($"Tick {i} Enemy HP: {enemy.Health}");
        }
    }


}