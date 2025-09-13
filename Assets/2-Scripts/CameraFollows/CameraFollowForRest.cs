using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraFollowForRest : MonoBehaviour
{
    public Transform player; 
    public float smoothSpeed = 2; 
    private float minX, maxX; 
    public float firstRoomStartTime;



    private void Start() {
        UpdateCameraSettings(1); 
        firstRoomStartTime = Time.time;
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
        if(SceneManager.GetActiveScene().name == "SnowScene"){

            switch (mapIndex)
            {
            case 1:
                minX = 2.7f; maxX = 5.3f;
                transform.position = new Vector3(minX, 3, -10); 
                break;
            case 2:
                minX = 30.78f; maxX = 41.2f;
                transform.position = new Vector3(minX, 2.1f, -10); 
                break;
            case 3:
                minX = 70.77f; maxX = 81.2f;
                transform.position = new Vector3(minX, 2.1f, -10); 
                break;
            case 4:
                minX = 104.93f; maxX = 104.93f;
                transform.position = new Vector3(minX, 2.1f, -10); 
                break;    
            }
        transform.position = new Vector3(minX, transform.position.y, -10);
        }

        else
        {

            switch (mapIndex)
            {
            case 1:
                minX = 0; maxX = 0;
                transform.position = new Vector3(minX, 0, -10); 
                break;
            case 2:
                minX = 26; maxX = 26;
                transform.position = new Vector3(minX, 0, -10); 
                break;
            case 3:
                minX = 52; maxX = 52;
                transform.position = new Vector3(minX, 0, -10); 
                break;
            case 4:
                minX = 78; maxX = 78;
                transform.position = new Vector3(minX, 0, -10); 
                break;    
            }
        transform.position = new Vector3(minX, transform.position.y, -10);
        }
        
    }

    public void TeleportToMap(int mapIndex)
    {
        UpdateCameraSettings(mapIndex);
    }
}