using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    private GameObject currentBuilding;

    // 🔥 Called when player clicks this spot
    void OnMouseDown()
    {
        // Prevent building on occupied spot
        if (currentBuilding != null)
            return;

        GameController.Instance.SelectBuildSpot(this);
    }

    // 🔥 Build and RETURN the created object
    public GameObject Build(GameObject prefab)
    {
        if (currentBuilding != null)
        {
            Debug.LogWarning("BuildSpot already occupied!");
            return null;
        }

        if (prefab == null)
        {
            Debug.LogError("No prefab selected to build!");
            return null;
        }

        currentBuilding = Instantiate(
            prefab,
            transform.position,
            Quaternion.identity
        );

        return currentBuilding;
    }

    // 🔥 Optional (future use: selling/upgrading)
    public void Clear()
    {
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
            currentBuilding = null;
        }
    }

    public bool IsOccupied()
    {
        return currentBuilding != null;
    }
}