using UnityEngine;

public class ScriptForHealthPotion : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(true);
        
    }
    private bool PlayerIsClose()
    {
        return Vector3.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) < 1.5f;
    }

    public void PickUp()
    {
        if(PlayerIsClose()){

            Destroy(gameObject);
        }
        
    }
}
