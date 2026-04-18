using NUnit.Framework;

public class MeleeTests
{
    [Test]
    public void Troop_CanAssignEnemy()
    {
        var troop = new MeleeTroop(100, 10, 1f, maxTargets: 1);
        var enemy = new Enemy(100);

        troop.AssignTarget(enemy);

        Assert.IsTrue(troop.IsEngagedWith(enemy), "Enemy was not assigned correctly");
    }

    [Test]
    public void Troop_RespectsMaxTargets()
    {
        var troop = new MeleeTroop(100, 10, 1f, maxTargets: 1);

        var e1 = new Enemy(100);
        var e2 = new Enemy(100);

        troop.AssignTarget(e1);
        troop.AssignTarget(e2);

        Assert.IsTrue(troop.IsEngagedWith(e1), "First enemy should be assigned");
        Assert.IsFalse(troop.IsEngagedWith(e2), "Second enemy should be rejected due to max targets");
    }

    [Test]
    public void Troop_AttacksAssignedEnemies_OncePerCooldown()
    {
        var troop = new MeleeTroop(100, 20, 1f, maxTargets: 1);
        var enemy = new Enemy(100);

        troop.AssignTarget(enemy);

        // FIRST ATTACK
        troop.Tick(1f);

        int afterFirstHit = enemy.Health;

        // SECOND TICK (should NOT double attack if cooldown works)
        troop.Tick(0.1f);

        int afterSecondTick = enemy.Health;

        Assert.AreEqual(80, afterFirstHit, "First attack should deal 20 damage");
        Assert.AreEqual(afterFirstHit, afterSecondTick, "Second tick should NOT deal damage (cooldown bug if it does)");
    }

    [Test]
    public void Troop_Regenerates_WhenNotInCombat()
    {
        var troop = new MeleeTroop(100, 10, 1f, maxTargets: 1);

        troop.TakeDamage(50);

        int beforeRegen = troop.Health;

        troop.Tick(5f);

        int afterRegen = troop.Health;

        Assert.Greater(afterRegen, beforeRegen, "Troop should regenerate when not in combat");
    }

    [Test]
    public void Troop_ShouldNotAttack_WhenNoTarget()
    {
        var troop = new MeleeTroop(100, 20, 1f, maxTargets: 1);

        int before = 100;

        troop.Tick(1f);

        Assert.AreEqual(before, troop.Health, "No target means no combat activity");
    }
}