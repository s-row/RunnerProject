using UnityEngine;

// 플레이어를 따라가되, 맵 밖이 보이지 않도록 카메라 위치를 제한
public class CameraControl : MonoBehaviour
{
    public Transform player;

    [Header("Map Limit")]
    public float mapMinX = -51f;
    public float mapMaxX = 50f;
    public float minY = -4.92f;
    public float maxY = 10f;

    // 플레이어 기준 카메라 위치 보정값
    public Vector3 moveAway = new Vector3(0, 1, -10);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 playerPosition = player.position + moveAway;

        // 현재 화면 비율에 맞춰 카메라가 실제로 보여주는 가로/세로 절반 크기 계산
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // 카메라 중심이 이동할 수 있는 최소/최대 X 좌표 계산
        float minCameraX = mapMinX + halfWidth;
        float maxCameraX = mapMaxX - halfWidth;

        // 맵 범위를 벗어나지 않도록 카메라 위치 제한
        float clampedX = Mathf.Clamp(playerPosition.x, minCameraX, maxCameraX);
        float clampedY = Mathf.Clamp(playerPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, moveAway.z);
    }
}