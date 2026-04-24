using UnityEngine;

public class PathNode : MonoBehaviour
{
    public bool isGuardPoint = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = isGuardPoint ? Color.red : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}