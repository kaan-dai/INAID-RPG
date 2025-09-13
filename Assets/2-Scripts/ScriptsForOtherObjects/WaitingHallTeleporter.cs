using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingHallTeleporter : MonoBehaviour
{
    [SerializeField] DungeonMasterInfoCollector dungeonMaster;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player.
        if (other.gameObject.CompareTag("Player"))
        {
            // If it is the player, teleport them to the target scene.
            dungeonMaster.pickARandomDungeon();
        }
    }
}