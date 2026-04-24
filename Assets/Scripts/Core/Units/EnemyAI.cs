using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Path path;
    public float speed = 2f;

    private int currentNodeIndex = 0;
    private EnemyView view;

    public float pathWidth = 1f; // how wide the path feels
    private float offset;
    private Vector3 currentTargetPos;
    private bool targetCalculated = false;

    void Awake()
    {
        view = GetComponent<EnemyView>();

        // Random offset per enemy
        offset = Random.Range(-pathWidth, pathWidth);
    }

    void Update()
    {
        // 🔥 HARD SAFETY: if script is disabled, do nothing
        if (!enabled) return;

        if (view == null || view.Data == null)
            return;

        // 🔥 HARD DEATH CHECK (prevents any logic after death)
        if (!view.Data.IsAlive || view.Data.State == Enemy.EnemyState.Dead)
        {
            enabled = false; // stop AI immediately
            return;
        }

        if (view.Data.State != Enemy.EnemyState.Moving)
            return;

        if (path == null || path.nodes == null || path.nodes.Length == 0)
            return;

        if (currentNodeIndex >= path.nodes.Length)
        {
            ReachBase();
            return;
        }

        Transform targetNode = path.nodes[currentNodeIndex];

        // 🔥 Calculate the offset target once per node so it doesn't circle
        if (!targetCalculated)
        {
            Vector3 dirToNode = (targetNode.position - transform.position).normalized;
            if (dirToNode == Vector3.zero) dirToNode = Vector3.up;

            // Perpendicular direction (2D)
            Vector3 perp = new Vector3(-dirToNode.y, dirToNode.x, 0f);
            currentTargetPos = targetNode.position + perp * offset;

            targetCalculated = true;
        }

        Vector3 moveDir = (currentTargetPos - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTargetPos) < 0.1f)
        {
            currentNodeIndex++;
            targetCalculated = false; // Reset so the next node calculates a new stable offset

            if (currentNodeIndex >= path.nodes.Length)
            {
                ReachBase();
            }
        }
    }

    void ReachBase()
    {
        Debug.Log("Enemy reached base!");

        // 🔥 Prevent any further AI execution this frame
        enabled = false;

        Destroy(gameObject);
    }
}