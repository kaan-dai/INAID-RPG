using UnityEngine;

public class CreditScripts : MonoBehaviour
{
    [SerializeField] private float verticalSpeed = 2f; 
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        
        float newY = initialPosition.y + Mathf.PingPong(Time.time * verticalSpeed, -100f); 
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}