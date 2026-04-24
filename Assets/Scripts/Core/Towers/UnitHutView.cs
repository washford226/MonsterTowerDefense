using UnityEngine;

public class UnitHutView : MonoBehaviour
{
    public GameObject troopPrefab;
    public Transform[] spawnPoints;

    [Header("Troop Stats")]
    public int troopCount = 3;
    public int health = 100;
    public int damage = 20;
    public float attackCooldown = 1f;
    public float moveSpeed = 2f;
    public float respawnTime = 5f;

    void Start()
    {
        var hut = CreateHut();
        Init(hut, GameController.Instance.sim);
    }

    public UnitHut CreateHut()
    {
        return new UnitHut(troopCount, health, damage, attackCooldown);
    }

    public void Init(UnitHut hut, CombatSimulation sim)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned to UnitHutView!");
            return;
        }

        for (int i = 0; i < hut.Troops.Count && i < spawnPoints.Length; i++)
        {
            SpawnTroop(hut.Troops[i], spawnPoints[i], sim);
        }
    }

    void SpawnTroop(MeleeTroop troopData, Transform spawn, CombatSimulation sim)
    {
        sim.Troops.Add(troopData);

        var go = Instantiate(troopPrefab, spawn.position, Quaternion.identity);

        var view = go.GetComponent<TroopView>();
        view.Init(troopData);

        // 🔥 Apply movement speed
        var ai = go.GetComponent<MeleeUnitAI>();
        if (ai != null)
        {
            ai.speed = moveSpeed;
        }
    }
}