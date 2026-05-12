using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    public GameObject panel;

    private BuildSpot currentSpot;

    public void Show(BuildSpot spot)
    {
        currentSpot = spot;

        panel.SetActive(true);

        // Convert world position -> screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(
            spot.transform.position
        );

        // Offset left of build spot
        screenPos.x -= 150f;

        panel.transform.position = screenPos;
    }
    public void Hide()
    {
        panel.SetActive(false);
        currentSpot = null;
    }

    public void BuildArcherTower()
    {
        if (currentSpot == null) return;

        GameController.Instance.BuildAtSpot(
            currentSpot,
            GameController.Instance.archerTowerPrefab
        );

        Hide();
    }

    public void BuildUnitHut()
    {
        if (currentSpot == null) return;

        GameController.Instance.BuildAtSpot(
            currentSpot,
            GameController.Instance.unitHutPrefab
        );

        Hide();
    }
}