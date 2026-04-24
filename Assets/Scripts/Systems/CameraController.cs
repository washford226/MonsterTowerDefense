using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float edgeScrollSpeed = 10f;
    public float edgeSize = 20f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 15f;

    [Header("Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampPosition();
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        // WASD / Arrow Keys
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move.y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move.y -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move.x += 1;

        // Mouse Edge Scrolling
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgeSize)
            move.x -= 1;
        if (mousePos.x >= Screen.width - edgeSize)
            move.x += 1;
        if (mousePos.y <= edgeSize)
            move.y -= 1;
        if (mousePos.y >= Screen.height - edgeSize)
            move.y += 1;

        transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        transform.position = pos;
    }
}