using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform healthBarTransform;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        if (playerTransform != null && healthBarTransform != null)
        {
            healthBarTransform.position = Camera.main.WorldToScreenPoint(playerTransform.position + offset);
        }
    }
}
