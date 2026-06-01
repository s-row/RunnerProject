using UnityEngine;

public class CamaraControl : MonoBehaviour
{
    public Transform player;

    public float mapMinX = -51f;
    public float mapMaxX = 50f;
    public float minY = -4.92f;
    public float maxY = 10f;

    public Vector3 moveAway = new Vector3(0, 1, -10);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 playerPosition = player.position + moveAway;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minCameraX = mapMinX + halfWidth;
        float maxCameraX = mapMaxX - halfWidth;

        float clampedX = Mathf.Clamp(playerPosition.x, minCameraX, maxCameraX);
        float clampedY = Mathf.Clamp(playerPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, moveAway.z);
    }
}
