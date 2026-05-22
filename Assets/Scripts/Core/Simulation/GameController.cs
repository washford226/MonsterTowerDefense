using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // Simulation
    public CombatSimulation sim;
    private Dictionary<MeleeTroop, TroopView> troopMap = new();
    public Dictionary<MeleeTroop, TroopView> TroopMap => troopMap;

    // Prefabs
    public GameObject arrowPrefab;
    public GameObject enemyPrefab;
    public GameObject archerTowerPrefab;
    public GameObject unitHutPrefab;

    // Scene refs
    public Transform spawnPoint;
    public Path path;

    // Systems
    private WaveManager waveManager;

    // Runtime tracking
    private List<ArcherTower> towers = new();
    private List<ArcherTowerView> towerViews = new();

    private Dictionary<Enemy, EnemyView> enemyMap = new();

    // Build system
    private BuildSpot selectedSpot;
    private GameObject selectedBuildingPrefab;
    public BuildMenuUI buildMenuUI;

    void Awake()
    {
        Instance = this;
    }

    public List<Wave> waves = new List<Wave>
    {
        new Wave(
            new List<WaveEnemyGroup> { 
                new WaveEnemyGroup { count = 20, spawnInterval = 1f, enemyType = "Basic" } 
            }, 
            false, 
            5f
        ),
        new Wave(
            new List<WaveEnemyGroup> { 
                new WaveEnemyGroup { count = 10, spawnInterval = 0.8f, enemyType = "Basic" },
                new WaveEnemyGroup { count = 5, spawnInterval = 1.2f, enemyType = "Elite" }
            }, 
            true, 
            5f
        ) // Boss spawns at end
    };

    void Start()
    {
        waveManager = new WaveManager(waves);
        sim = new CombatSimulation(waveManager);
    }

    void Update()
    {
        sim.Tick(Time.deltaTime);

        UpdateEnemyCombatDistances();

        HandleWaves();
        UpdateTowers();
    }

    // =========================
    // WAVES
    // =========================
    void HandleWaves()
    {
        Enemy enemyData = waveManager.Tick(Time.deltaTime);

        if (enemyData != null)
        {
            SpawnEnemy(enemyData);
        }
    }

    // =========================
    // TOWERS
    // =========================
    void UpdateTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            var tower = towers[i];
            var view = towerViews[i];

            tower.Tick(Time.deltaTime);

            Enemy target = tower.Attack(
                sim.Enemies,
                enemyMap,
                view.transform.position
            );

            if (target != null && enemyMap.TryGetValue(target, out EnemyView ev))
            {
                view.SetTarget(ev.transform);
            }
            else
            {
                view.SetTarget(null);
            }
        }
    }

    public void RegisterTroop(MeleeTroop troop, TroopView view)
    {
        troopMap[troop] = view;
    }

    // =========================
    // ENEMIES
    // =========================
    void SpawnEnemy(Enemy enemyData)
    {
        GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyAI ai = obj.GetComponent<EnemyAI>();
        ai.path = path;

        EnemyView view = obj.GetComponent<EnemyView>();
        Enemy runtimeEnemy = view.CreateEnemy();

        view.Initialize(runtimeEnemy);

        RegisterEnemy(runtimeEnemy, view);

        sim.Enemies.Add(runtimeEnemy);

        view.OnDeath += HandleEnemyDeath;
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        Debug.Log("GC: Cleaning enemy");

        UnregisterEnemy(enemy);

        if (sim.Enemies.Contains(enemy))
            sim.Enemies.Remove(enemy);
    }

    void UpdateEnemyCombatDistances()
    {
        foreach (var pair in enemyMap)
        {
            Enemy enemy = pair.Key;
            EnemyView view = pair.Value;

            if (enemy == null || view == null)
                continue;

            enemy.CheckCombatDistance(
                troopMap,
                view.transform.position
            );
        }
    }

    public Vector3 GetEnemyPosition(Enemy enemy)
    {
        if (enemyMap.TryGetValue(enemy, out EnemyView view))
        {
            return view.transform.position;
        }

        return Vector3.zero;
    }

    // =========================
    // ARROWS
    // =========================
    void SpawnArrow(Enemy enemy, ArcherTowerView sourceView)
    {
        if (sourceView == null) return;

        var arrowGO = Instantiate(
            arrowPrefab,
            sourceView.firePoint.position,
            Quaternion.identity
        );

        var arrowView = arrowGO.GetComponent<ArrowView>();

        if (enemyMap.TryGetValue(enemy, out EnemyView enemyView))
        {
            arrowView.Init(enemyView, 0.5f);
        }
        else
        {
            Debug.LogWarning("EnemyView not found for enemy!");
        }
    }

    // =========================
    // BUILD SYSTEM
    // =========================

    public void BuildAtSpot(BuildSpot spot, GameObject prefab)
    {
        GameObject obj = spot.Build(prefab);

        if (obj == null)
            return;

        // Archer Tower setup
        ArcherTowerView towerView = obj.GetComponent<ArcherTowerView>();

        if (towerView != null)
        {
            ArcherTower tower = towerView.CreateTower();

            towerView.Init(tower);

            RegisterTower(towerView, tower);
        }

        // Unit Hut setup
        UnitHutView hutView = obj.GetComponent<UnitHutView>();

        if (hutView != null)
        {
            UnitHut hut = hutView.CreateHut();

            hutView.Init(hut, sim);
        }
    }

    // =========================
    // TOWER REGISTRATION
    // =========================
    public void RegisterTower(ArcherTowerView view, ArcherTower tower)
    {
        towers.Add(tower);
        towerViews.Add(view);

        tower.OnShoot += (enemy) =>
        {
            SpawnArrow(enemy, view);
        };

        sim.Towers.Add(tower);
    }

    // =========================
    // MAPPING
    // =========================
    public void RegisterEnemy(Enemy enemy, EnemyView view)
    {
        enemyMap[enemy] = view;
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemyMap.Remove(enemy);
    }

    public Dictionary<MeleeTroop, TroopView> GetTroopMap()
    {
        return troopMap;
    }

    public bool TryGetEnemyView(Enemy enemy, out EnemyView view)
    {
        return enemyMap.TryGetValue(enemy, out view);
    }
}