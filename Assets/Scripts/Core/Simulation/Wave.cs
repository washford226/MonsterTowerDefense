using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct WaveEnemyGroup
{
    public int count;
    public float spawnInterval;
    public string enemyType; // Support strings or enums for now, later mapped to prefabs
}

[System.Serializable]
public class Wave
{
    public List<WaveEnemyGroup> EnemyGroups = new List<WaveEnemyGroup>();
    public bool SpawnBossAtEnd = false;
    public float InterWaveDelay = 5f;

    // We can keep a simplified constructor for testing, or rely on the inspector
    public Wave() { }

    public Wave(List<WaveEnemyGroup> groups, bool spawnBoss, float delay)
    {
        EnemyGroups = groups;
        SpawnBossAtEnd = spawnBoss;
        InterWaveDelay = delay;
    }
}