using UnityEngine;

using System.Collections.Generic;

public class WaveManager
{
    private List<Wave> waves = new List<Wave>();
    private int currentWaveIndex = 0;

    private float spawnTimer = 0f;
    private float delayTimer = 0f;

    private int currentGroupIndex = 0;
    private int spawnedEnemiesInGroup = 0;
    private bool isWaitingForNextWave = false;
    private bool bossSpawnedForCurrentWave = false;

    // Track active enemies if needed for delay (currently delay just ticks on time)

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

        if (isWaitingForNextWave)
        {
            delayTimer += deltaTime;
            if (delayTimer >= wave.InterWaveDelay)
            {
                delayTimer = 0f;
                isWaitingForNextWave = false;
                currentWaveIndex++;
                ResetWaveState();
            }
            return null;
        }

        spawnTimer += deltaTime;

        // Still spawning regular groups
        if (currentGroupIndex < wave.EnemyGroups.Count)
        {
            var group = wave.EnemyGroups[currentGroupIndex];

            if (spawnedEnemiesInGroup < group.count && spawnTimer >= group.spawnInterval)
            {
                spawnTimer = 0f;
                spawnedEnemiesInGroup++;

                return SpawnEnemyForWave(group.enemyType);
            }

            // Group finished
            if (spawnedEnemiesInGroup >= group.count)
            {
                currentGroupIndex++;
                spawnedEnemiesInGroup = 0;
            }

            return null;
        }

        // All groups finished, spawn boss if available
        if (wave.SpawnBossAtEnd && !bossSpawnedForCurrentWave)
        {
             bossSpawnedForCurrentWave = true;
             return SpawnEnemyForWave("Boss");
        }

        // Wave fully spawned, start waiting delay for next wave
        isWaitingForNextWave = true;

        return null;
    }

    private void ResetWaveState()
    {
        currentGroupIndex = 0;
        spawnedEnemiesInGroup = 0;
        spawnTimer = 0f;
        bossSpawnedForCurrentWave = false;
    }

    private Enemy SpawnEnemyForWave(string type)
    {
        if (type == "Boss")
        {
            return new Enemy(
                500,   // health
                20,    // damage
                2f,    // attack cooldown
                2f     // engage range
            );
        }
        else if (type == "Elite")
        {
            return new Enemy(
                200,
                10,
                1.5f,
                1.75f
            );
        }

        // Basic enemy
        return new Enemy(
            100,
            5,
            1f,
            1.5f
        );
    }
}