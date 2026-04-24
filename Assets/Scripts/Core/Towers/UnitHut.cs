using UnityEngine;

using System.Collections.Generic;

public class UnitHut
{
    public List<MeleeTroop> Troops { get; private set; } = new List<MeleeTroop>();

    public UnitHut(int troopCount, int health, int damage, float attackCooldown)
    {
        for (int i = 0; i < troopCount; i++)
        {
            var troop = new MeleeTroop(health, damage, attackCooldown, maxTargets: 1);
            Troops.Add(troop);
        }
    }
}