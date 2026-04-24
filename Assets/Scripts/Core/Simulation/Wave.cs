using UnityEngine;

public class Wave
{
    public int EnemyCount;
    public float SpawnInterval;

    public Wave(int enemyCount, float spawnInterval)
    {
        EnemyCount = enemyCount;
        SpawnInterval = spawnInterval;
    }
}