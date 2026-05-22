using UnityEngine;

using System.Collections.Generic;

public class CombatSimulation
{
    public List<Enemy> Enemies = new List<Enemy>();
    public List<MeleeTroop> Troops = new List<MeleeTroop>();
    public List<ArcherTower> Towers = new List<ArcherTower>();
    public List<Arrow> Arrows = new List<Arrow>();
    public WaveManager WaveManager;

    public CombatSimulation(WaveManager waveManager)
    {
        WaveManager = waveManager;
    }

    public void Tick(float deltaTime)
    {

        // 🔥 Remove any already dead or destroyed ones
        Enemies.RemoveAll(e => e == null || !e.IsAlive);

        foreach (var enemy in Enemies)
        {
            if (GameController.Instance == null)
                continue;

            if (!GameController.Instance.TryGetEnemyView(enemy, out EnemyView enemyView))
                continue;

            enemy.TryEngage(
                Troops,
                GameController.Instance.GetTroopMap(),
                enemyView.transform.position
            );

            enemy.CheckCombatDistance(
                GameController.Instance.GetTroopMap(),
                enemyView.transform.position
            );

            enemy.Tick(deltaTime);
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