using UnityEngine;

public class MeleeUnitAI : MonoBehaviour
{
    public float speed = 2f;
    private Transform targetNode;

    void Start()
    {
        FindClosestGuardPoint();
    }

    void Update()
    {
        if (targetNode == null) return;

        float dist = Vector3.Distance(transform.position, targetNode.position);

        if (dist < 0.1f)
            return; // 🔥 stop moving once reached

        Vector3 dir = (targetNode.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    void FindClosestGuardPoint()
    {
        var nodes = GuardPointManager.Instance.guardPoints;

        float closestDist = Mathf.Infinity;

        foreach (var node in nodes)
        {
            if (!node.isGuardPoint) continue;

            float dist = Vector3.Distance(transform.position, node.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                targetNode = node.transform;
            }
        }
    }
}