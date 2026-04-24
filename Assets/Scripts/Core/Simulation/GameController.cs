using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // Simulation
    public CombatSimulation sim;

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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        sim = new CombatSimulation();

        waveManager = new WaveManager(new List<Wave>()
        {
            new Wave(5, 1f),
            new Wave(8, 0.8f),
            new Wave(12, 0.6f)
        });
    }

    void Update()
    {
        sim.Tick(Time.deltaTime);

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

    // =========================
    // ENEMIES
    // =========================
    void SpawnEnemy(Enemy enemyData)
    {
        GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyAI ai = obj.GetComponent<EnemyAI>();
        ai.path = path;

        EnemyView view = obj.GetComponent<EnemyView>();
        view.Initialize(enemyData);

        RegisterEnemy(enemyData, view);

        sim.Enemies.Add(enemyData);

        view.OnDeath += HandleEnemyDeath;
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        Debug.Log("GC: Cleaning enemy");

        UnregisterEnemy(enemy);

        if (sim.Enemies.Contains(enemy))
            sim.Enemies.Remove(enemy);
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
    public void SelectBuildSpot(BuildSpot spot)
    {
        selectedSpot = spot;

        // TEMP default selection
        selectedBuildingPrefab = archerTowerPrefab;

        BuildSelected();
    }

    void BuildSelected()
    {
        if (selectedSpot == null || selectedBuildingPrefab == null)
            return;

        GameObject built = selectedSpot.Build(selectedBuildingPrefab);

        // 🔥 Handle Archer Tower
        ArcherTowerView towerView = built.GetComponent<ArcherTowerView>();
        if (towerView != null)
        {
            ArcherTower tower = towerView.CreateTower();
            RegisterTower(towerView, tower);
        }

        // 🔥 (Future) Handle Unit Hut here
        UnitHutView hutView = built.GetComponent<UnitHutView>();
        if (hutView != null)
        {
            var hut = new UnitHut(3, 100, 20, 1f);
            hutView.Init(hut, sim);
        }

        selectedSpot = null;
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
}