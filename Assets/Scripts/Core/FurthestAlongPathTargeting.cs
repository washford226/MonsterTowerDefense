using UnityEngine;

using System.Collections.Generic;
using System.Linq;

public class FurthestAlongPathTargeting : ITargetingStrategy
{
    public Enemy SelectTarget(List<Enemy> enemies)
    {
        if (enemies == null || enemies.Count == 0)
            return null;

        return enemies
            .Where(e => e.IsAlive)
            .OrderByDescending(e => e.PathProgress)
            .FirstOrDefault();
    }
}