using UnityEngine;

using System.Collections.Generic;

public class WaveManager
{
    private List<Wave> waves = new List<Wave>();
    private int currentWaveIndex = 0;

    private float spawnTimer = 0f;
    private int spawnedEnemies = 0;

    public bool IsFinished => currentWaveIndex >= waves.Count;

    public WaveManager(List<Wave> waves)
    {
        this.waves = waves;
    }

    public Enemy Tick(float deltaTime)
    {
        if (IsFinished)
            return null;

        var wave = waves[currentWaveIndex];

        spawnTimer += deltaTime;

        if (spawnedEnemies < wave.EnemyCount &&
            spawnTimer >= wave.SpawnInterval)
        {
            spawnTimer = 0f;
            spawnedEnemies++;

            return SpawnEnemyForWave();
        }

        if (spawnedEnemies >= wave.EnemyCount)
        {
            currentWaveIndex++;
            spawnedEnemies = 0;
            spawnTimer = 0f;
        }

        return null;
    }

    private Enemy SpawnEnemyForWave()
    {
        // basic placeholder enemy
        return new Enemy(100);
    }
}