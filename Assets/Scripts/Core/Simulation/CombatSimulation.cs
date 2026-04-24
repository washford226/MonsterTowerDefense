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

        // Remove dead enemies, BUT don't block them from being processed in the frame they die 
        // if they need destruction handling. Wait, actually, removing them here means they are never ticked?
        // No, Enemies is just a list.

        if (spawnedEnemy != null)
        {
            Enemies.Add(spawnedEnemy);
        }

        // 🔥 Remove any already dead or destroyed ones
        Enemies.RemoveAll(e => e == null || !e.IsAlive);

        foreach (var enemy in Enemies)
        {
            enemy.TryEngage(Troops);
        }

        foreach (var troop in Troops)
        {
            troop.Tick(deltaTime);
        }

        for (int i = Arrows.Count - 1; i >= 0; i--)
        {
            Arrows[i].Tick(deltaTime);

            if (Arrows[i].IsComplete)
            {
                Arrows.RemoveAt(i);
            }
        }
    }
}