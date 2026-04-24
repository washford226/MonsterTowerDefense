using UnityEngine;

public class GuardPointManager : MonoBehaviour
{
    public static GuardPointManager Instance;

    public PathNode[] guardPoints;

    void Awake()
    {
        Instance = this;
        guardPoints = FindObjectsOfType<PathNode>();
    }
}