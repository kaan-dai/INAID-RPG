using UnityEngine;

public class CameraFollowWaitingHall : MonoBehaviour
{
    public Transform player; 
    public float smoothSpeed = 2; 
    private float minX, maxX; 

    private void Start() {
        UpdateCameraSettings(1); 
    }

    private void LateUpdate()
    {
        float desiredX = Mathf.Clamp(player.position.x, minX, maxX);
        Vector3 desiredPosition = new Vector3(desiredX, transform.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void UpdateCameraSettings(int mapIndex)
    {
        switch (mapIndex)
        {
            case 1:
                minX = -4.41f; maxX = -4.41f;
                transform.position = new Vector3(minX, -2.67f, -10); 
                break;
            case 2:
                minX = 26; maxX = 26;
                transform.position = new Vector3(minX, 0, -10); 
                break;
            case 3:
                minX = 52; maxX = 52;
                transform.position = new Vector3(minX, 0, -10); 
                break;
        }
        transform.position = new Vector3(minX, transform.position.y, -10);
    }

    public void TeleportToMap(int mapIndex)
    {
        UpdateCameraSettings(mapIndex);
    }
}
