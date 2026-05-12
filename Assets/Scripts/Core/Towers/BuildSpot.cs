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

        GameController.Instance.buildMenuUI.Show(this);
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

        // 🔥 Hide build spot visuals
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        // 🔥 Disable clicking
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

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

        // 🔥 Re-enable visuals
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }
    public bool IsOccupied()
    {
        return currentBuilding != null;
    }
}