using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject pfHealthBarUIWorldCanvas;


    //Dungeon 1 = Crystal
    //Dungeon 2 = Desert
    //Dungeon 3 = Rocky
    //Dungeon 4 = Snow
    public void SpawnerSpawnEntity(string dungeonName, int roomNumber)
    {
        switch(dungeonName)
        {
            case "Crystal":
            if(roomNumber == 1)
            {
                
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForFlyingEnemy enemy = enemyObject.GetComponent<ScriptForFlyingEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);
            }
            else if(roomNumber == 2)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForRaEnemy enemy = enemyObject.GetComponent<ScriptForRaEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);
                
            }
            break;
            case "Desert":
            if(roomNumber == 1)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForSnakeManEnemy enemy = enemyObject.GetComponent<ScriptForSnakeManEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);

            }
            else if(roomNumber == 2)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForSnakeManEnemy enemy = enemyObject.GetComponent<ScriptForSnakeManEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);

            }
            break;
            case "Rocky":
            if(roomNumber == 1)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForRedHeadEnemy enemy = enemyObject.GetComponent<ScriptForRedHeadEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);

            }
            else if(roomNumber == 2)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForRedHeadEnemy enemy = enemyObject.GetComponent<ScriptForRedHeadEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);
            }
            break;
            case "Snow":
            if(roomNumber == 1)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForGreenEnemy enemy = enemyObject.GetComponent<ScriptForGreenEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);
            }
            else if(roomNumber == 2)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, transform.position, transform.rotation);
                ScriptForGreenEnemy enemy = enemyObject.GetComponent<ScriptForGreenEnemy>();
                enemy.leftBound = leftBound;
                enemy.rightBound = rightBound;
                enemy.playerTransform = playerTransform;
                GameObject healthBarCanvas = Instantiate(pfHealthBarUIWorldCanvas);
                healthBarCanvas.SetActive(false);
            }
            break;
            default:

            break;

        }
    }
}