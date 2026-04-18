using UnityEngine;

using System.Collections.Generic;

public interface ITargetingStrategy
{
    Enemy SelectTarget(List<Enemy> enemies);
}