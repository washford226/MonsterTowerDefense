using UnityEngine;

using System.Collections.Generic;

public class CombatSimulation
{
    public List<Enemy> Enemies = new List<Enemy>();
    public List<MeleeTroop> Troops = new List<MeleeTroop>();
    public List<ArcherTower> Towers = new List<ArcherTower>();
    public List<Arrow> Arrows = new List<Arrow>();
    public WaveManager WaveManager;

    public CombatSimulation()
    {
        WaveManager = new WaveManager(new List<Wave>
        {
            new Wave(5, 1f),
            new Wave(10, 0.8f),
            new Wave(15, 0.5f)
        });
    }

    public void Tick(float deltaTime)
    {
        var spawnedEnemy = WaveManager.Tick(deltaTime);

        if (spawnedEnemy != null)
        {
            Enemies.Add(spawnedEnemy);
        }

        // 1. ENEMY ENGAGEMENT
        foreach (var enemy in Enemies)
        {
            enemy.TryEngage(Troops);
        }

        // 2. UPDATE ENEMIES (movement/combat state later)
        foreach (var enemy in Enemies)
        {
            // placeholder for movement system later
        }

        // 3. UPDATE TROOPS
        foreach (var troop in Troops)
        {
            troop.Tick(deltaTime);
        }

        // 4. UPDATE TOWERS
        foreach (var tower in Towers)
        {
            tower.Tick(deltaTime);

            var target = tower.Attack(Enemies);

            if (target != null)
            {
                UnityEngine.Debug.Log("Tower fired at enemy!");

                Arrows.Add(new Arrow(target, tower.Damage, travelTime: 0.5f));
            }
        }

        // 5. UPDATE ARROWS
        for (int i = Arrows.Count - 1; i >= 0; i--)
        {
            Arrows[i].Tick(deltaTime);

            if (Arrows[i].IsComplete)
            {
                Arrows.RemoveAt(i);
            }
        }

        // 6. CLEANUP DEAD ENEMIES
        Enemies.RemoveAll(e => !e.IsAlive);
    }
}