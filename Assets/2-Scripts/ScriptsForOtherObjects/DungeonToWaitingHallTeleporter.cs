using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonToWaitingHallTeleporter : MonoBehaviour
{
    [SerializeField] DungeonMasterInfoCollector dungeonMaster;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player.
        if (other.gameObject.CompareTag("Player"))
        {
            // If it is the player, teleport them to the target scene.
            dungeonMaster.returnToWaitingHall();
        }
    }
}