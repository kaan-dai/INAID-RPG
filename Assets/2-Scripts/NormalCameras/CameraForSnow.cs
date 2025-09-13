using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// İce World map 1 camera posiitons x:5 y:3 z:-10 size: 6.048985
// İce World map 2 camera posiitons x:30.78 y:2.1 z:-10 size: 6.048985
// İce World map 3 camera posiitons x:70.77 y:2.8 z:-10 size: 6.048985

//Crystal world map 1 camera posiitons x:0 y:0 z:-10 size: 6.048985
//Crystal world map 2 camera posiitons x:26 y:0 z:-10 size: 6.048985
//Crystal world map 3 camera posiitons x:52 y:0 z:-10 size: 6.048985

//Rocky world map 1 camera posiitons x:0 y:0 z:-10 size: 6.048985
//Rocky world map 2 camera posiitons x:26 y:0 z:-10 size: 6.048985
//Rocky world map 3 camera posiitons x:52 y:0 z:-10 size: 6.048985

//Desert world map 1 camera posiitons x:0 y:0 z:-10 size: 6.048985
//Desert world map 1 camera posiitons x:26 y:0 z:-10 size: 6.048985
//Desert world map 1 camera posiitons x:52 y:0 z:-10 size: 6.048985




public class CameraForSnow : MonoBehaviour
{
    public Transform player;  
   
    //private float map1X = 0.0f;
    private float map2X1 = 30.78f;
    private float map2Y1 = 2.1f;
    private float map2X2 = 41.29f;
    private float map2Y2 = 2.1f;
    private float map3X1 = 70.91f; 
    private float map3Y1 = 2.1f; 
    private float map3X2 = 81.26f; 
    private float map3Y2 = 2.1f;

    void Start()
    {
        if (player == null)
        {
           
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    void Update()
    {
        if (player.position.x >= 15 && player.position.x < 17)
        {
            
            transform.position = new Vector3(map2X1, map2Y1, transform.position.z);
        }
        else if (player.position.x >= 41.66f && player.position.x < 42)
        {
            transform.position = new Vector3(map2X2, map2Y2, transform.position.z);
        }
        else if (player.position.x >= 51.5f & player.position.x < 53)
        {
            transform.position = new Vector3(map3X1, map3Y1, transform.position.z);
        }
        else if (player.position.x >= 81.82f & player.position.x < 82)
        {
            transform.position = new Vector3(map3X2, map3Y2, transform.position.z); 
        }
    }
}

