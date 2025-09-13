using UnityEngine;

public class ScriptForSpikes : MonoBehaviour
{
[SerializeField] private float damage = 1.5f;  
    private float damageInterval = 1.5f; 
    private float lastDamageTime; 

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (Time.time >= lastDamageTime + damageInterval)
            {
                lastDamageTime = Time.time; 
                ScriptForPlayerRest playerScript = collision.gameObject.GetComponent<ScriptForPlayerRest>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(damage);
                }
            }
        }
    }
}
