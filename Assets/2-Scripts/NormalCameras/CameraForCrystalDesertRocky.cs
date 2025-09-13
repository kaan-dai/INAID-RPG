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
public class CameraForCrystalDesertRocky : MonoBehaviour
{
    public Transform player;    

   
    //private float map1X = 0.0f;
    private float map2X = 26.0f;
    private float map3X = 52.0f;  

    void Update()
    {
        if (player.position.x >= 10.65f && player.position.x < 11)
        {
            transform.position = new Vector3(map2X, transform.position.y, transform.position.z);
        }
        else if (player.position.x >= 36.88f && player.position.x < 37.5f)
        {
            transform.position = new Vector3(map3X, transform.position.y, transform.position.z);
        }
    }
}
