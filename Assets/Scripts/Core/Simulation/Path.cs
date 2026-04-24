using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] nodes;

    private void Awake()
    {
        var all = GetComponentsInChildren<Transform>();

        // 🔥 remove self (index 0 is always this object)
        nodes = new Transform[all.Length - 1];

        for (int i = 1; i < all.Length; i++)
        {
            nodes[i - 1] = all[i];
        }
    }

    public Transform GetNode(int index)
    {
        if (index < 0 || index >= nodes.Length) return null;
        return nodes[index];
    }

    public int Length => nodes.Length;

    private void OnDrawGizmos()
    {
        var all = GetComponentsInChildren<Transform>();

        if (all == null || all.Length < 3) return;

        Gizmos.color = Color.green;

        for (int i = 2; i < all.Length - 1; i++)
        {
            if (all[i] != null && all[i + 1] != null)
            {
                Gizmos.DrawLine(all[i].position, all[i + 1].position);
            }
        }
    }
}