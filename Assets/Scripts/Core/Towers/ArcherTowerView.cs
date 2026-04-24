using UnityEngine;

public class ArcherTowerView : MonoBehaviour
{
    public Transform firePoint;

    [Header("Tower Stats")]
    public float range = 3f;
    public int damage = 10;
    public float attackCooldown = 1f;

    private ArcherTower tower;
    private Transform currentTarget;

    void Start()
    {
        var towerData = CreateTower();
        Init(towerData);

        GameController.Instance.RegisterTower(this, towerData);
    }

    public void Init(ArcherTower towerData)
    {
        tower = towerData;
    }

    public ArcherTower CreateTower()
    {
        return new ArcherTower(
            range,
            damage,
            attackCooldown,
            new FurthestAlongPathTargeting()
        );
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    // 🔥 DISABLED for now (no rotation/movement)
    void Update()
    {
        return;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}