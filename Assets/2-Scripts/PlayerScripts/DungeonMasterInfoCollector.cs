using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DungeonMasterInfoCollector : MonoBehaviour
{
    [SerializeField] GameObject player;

    private static int maxEnemyToSpawn = 10;

    Dialogue newNarration;
    string dungeonName;
    int roomNumber;
    string roomName;
    string monstersInTheRoomInfo;
    PartyScript party;
    string partyInfo;
    int dungeonCount = 0;
    int randomDungeon = 0;

    [SerializeField] CompanionAI companionAi1;
    [SerializeField] CompanionAI companionAi2;
    [SerializeField] CompanionAI companionAi3;

    [SerializeField] public ChatGPT gpt1;
    [SerializeField] public ChatGPT gpt2;
    [SerializeField] public ChatGPT gpt3;

    [SerializeField] private GameObject sceneName;
    public static Dictionary<int, bool> maps = new Dictionary<int, bool>();

    private int spawnedEnemyNum;

    [SerializeField] private GameObject companion1GptForBlocking;

    //Dungeon 1 = Crystal
    //Dungeon 2 = Desert
    //Dungeon 3 = Rocky
    //Dungeon 4 = Snow

    // Start is called before the first frame update
    void Start()
    {
        companionAi1 = GameObject.Find("MartialHero").GetComponent<CompanionAI>();
        companionAi2 = GameObject.Find("Mage").GetComponent<CompanionAI>();
        companionAi3 = GameObject.Find("Archer").GetComponent<CompanionAI>();
        this.roomNumber = 1;
        newNarration = FindObjectOfType<Dialogue>();
        maps.Add(1, false);
        maps.Add(2, false);
        maps.Add(3, false);
        maps.Add(4, false);
    }

    // "Snow Dungeon, Room 2, 2 Monsters:Bow user, sword user, Party Members: Solo"
    public void callNarrator(int eventNumber){
        dungeonName = sceneName.name + "Dungeon,";

        switch(eventNumber){
            case 0:
            if(roomNumber < 4)
            {
                roomName = " Area " + this.roomNumber + ", ";
                monstersInTheRoomInfo = spawnedEnemyNum + " Monsters. Type: " + getMonsterInfo();
                partyInfo = player.GetComponent<PartyScript>().getPartyInfo();
                newNarration.ReceiveSystemMessage(dungeonName + roomName + monstersInTheRoomInfo + partyInfo);
                gpt1.SetEnvironmentInfo("You are currently in the " + dungeonName + " the description of the enemies around is " + getMonsterInfo(), " your task is to kill the " + sceneName.name + " boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                gpt2.SetEnvironmentInfo("You are currently in the " + dungeonName + " the description of the enemies around is " + getMonsterInfo(), " your task is to kill the " + sceneName.name + " boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                gpt3.SetEnvironmentInfo("You are currently in the " + dungeonName + " the description of the enemies around is " + getMonsterInfo(), " your task is to kill the " + sceneName.name + " boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
            }
            else if(roomNumber == 4)
            {
                newNarration.ReceiveSystemMessage("You are against the " + dungeonName + " boss." + "  Goddess blessed you, you can jump higher." );
                gpt1.SetEnvironmentInfo("You are now in the " + sceneName.name + " boss room.", "your task is to kill the " + sceneName.name + " boss");
                gpt2.SetEnvironmentInfo("You are now in the " + sceneName.name + " boss room.", "your task is to kill the " + sceneName.name + " boss");
                gpt3.SetEnvironmentInfo("You are now in the " + sceneName.name + " boss room.", "your task is to kill the " + sceneName.name + " boss");
            }
            break;
            case 1:
                    newNarration.ReceiveSystemMessage("You killed the " + dungeonName +  "boss");
                    gpt1.SetEnvironmentInfo("You killed the boss of this dungeon.", "Since you killed the boss, your task is to rest for now.");
                    gpt2.SetEnvironmentInfo("You killed the boss of this dungeon.", "Since you killed the boss, your task is to rest for now.");
                    gpt3.SetEnvironmentInfo("You killed the boss of this dungeon.", "Since you killed the boss, your task is to rest for now.");
                if (dungeonCount == 4)
                    {
                    dungeonCount = 0;
                    } 
                break;

            case 2:

            break;

            case 3:
                if (!gpt1.GetActiveMessage()) {
                    List<int> intList = new List<int>();
                    

                    foreach (KeyValuePair<int, bool> mapValues in maps)
                    {
                        if (mapValues.Value == false)
                        {
                            intList.Add(mapValues.Key);
                        }
                    }
                    
                    if(intList.Count == 0){
                        gpt1.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                        SceneManager.LoadScene("CreditsScene");
                        return;
                    }
                    
                    

                    randomDungeon = GetRandomNumber(intList);
                    
                    dungeonName = sceneName.name + "Dungeon,";
                    if (randomDungeon == 1)
                    {
                        roomNumber = 1;
                        SceneManager.LoadScene("CrystalScene");
                        gpt1.SetEnvironmentInfo("You are being sent to the Crystal Dungeon, the description of the enemies around is " + getMonsterInfo("Crystal"), " kill the Crystal boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt2.SetEnvironmentInfo("You are being sent to the Crystal Dungeon, the description of the enemies around is " + getMonsterInfo("Crystal"), " kill the Crystal boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt3.SetEnvironmentInfo("You are being sent to the Crystal Dungeon, the description of the enemies around is " + getMonsterInfo("Crystal"), " kill the Crystal boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        maps[randomDungeon] = true;
                    }
                    else if (randomDungeon == 2)
                    {
                        roomNumber = 1;
                        SceneManager.LoadScene("DesertScene");
                        gpt1.SetEnvironmentInfo("You are being sent to the Desert Dungeon, the description of the enemies around is " + getMonsterInfo("Desert"), " kill the Desert boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt2.SetEnvironmentInfo("You are being sent to the Desert Dungeon, the description of the enemies around is " + getMonsterInfo("Desert"), " kill the Desert boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt3.SetEnvironmentInfo("You are being sent to the Desert Dungeon, the description of the enemies around is " + getMonsterInfo("Desert"), " kill the Desert boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        maps[randomDungeon] = true;
                    }
                    else if (randomDungeon == 3)
                    {
                        roomNumber = 1;
                        SceneManager.LoadScene("RockyScene");
                        gpt1.SetEnvironmentInfo("You are being sent to the Rocky Dungeon, the description of the enemies around is " + getMonsterInfo("Rocky"), " kill the Rocky boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt2.SetEnvironmentInfo("You are being sent to the Rocky Dungeon, the description of the enemies around is " + getMonsterInfo("Rocky"), " kill the Rocky boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt3.SetEnvironmentInfo("You are being sent to the Rocky Dungeon, the description of the enemies around is " + getMonsterInfo("Rocky"), " kill the Rocky boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        maps[randomDungeon] = true;
                    }
                    else if (randomDungeon == 4)
                    {
                        roomNumber = 1;
                        SceneManager.LoadScene("SnowScene");
                        gpt1.SetEnvironmentInfo("You are being sent to the Snow Dungeon the description of the enemies around is " + getMonsterInfo("Snow"), " kill the Snow boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt2.SetEnvironmentInfo("You are being sent to the Snow Dungeon the description of the enemies around is " + getMonsterInfo("Snow"), " kill the Snow boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        gpt3.SetEnvironmentInfo("You are being sent to the Snow Dungeon the description of the enemies around is " + getMonsterInfo("Snow"), " kill the Snow boss, kill the smaller enemies as you get to the boss room, you will be informed when you are in the same room with the boss");
                        maps[randomDungeon] = true;
                    }
                }

                break;

            case 4:
                if (!gpt1.GetActiveMessage()) {
                    SceneManager.LoadScene("WaitingHallScene");
                    gpt1.SetEnvironmentInfo("You are being sent to the waiting hall.", "Chill for now.");
                    gpt2.SetEnvironmentInfo("You are being sent to the waiting hall.", "Chill for now.");
                    gpt3.SetEnvironmentInfo("You are being sent to the waiting hall.", "Chill for now.");
                }
                break;
            default:
            break;
        }
    }

    public void newRoom(){
        this.roomNumber++;
        companionAi1.StopAttacking();
        companionAi2.StopAttacking();
        companionAi3.StopAttacking();
        callNarrator(0);
    }

   public void killedBoss(){
        
        dungeonCount++;
        callNarrator(1);
    }

    public void getItem(){
        callNarrator(2);
    }

    public void pickARandomDungeon(){
        this.roomNumber = 1;
        callNarrator(3);
    }

    public void returnToWaitingHall()
    {
        callNarrator(4);
    }

    public string getMonsterInfo(string b = "WaitingHall")
    {
        string a = sceneName.name;

        if (a == "WaitingHall") {
            a = b;
        }

        switch (b)
        {
            case "Crystal":
            if(roomNumber == 1)
            {
                return "Flying green ghouls";
            }
            else if(roomNumber == 2)
            {
                return "Enemy with spear";
            }
            break;
            case "Desert":
            if(roomNumber == 1)
            {
                return "Carnivorous plants";
            }
            else if(roomNumber == 2)
            {
                return "Snakeman enemy with dagger";
            }
            break;
            case "Rocky":
            if(roomNumber == 1)
            {
                return "Red head enemy with knife";
            }
            else if(roomNumber == 2)
            {
                return "Red head enemy with knife";
            }
            break;
            case "Snow":
            if(roomNumber == 1)
            {
                return "Flying Vicious Owl";
            }
            else if(roomNumber == 2)
            {
                return "Green goo monster";
            }
            break;
            default:

            break;

        };
        return "";
    }

    public void ChangeDifficultyForNextRoom(float currentRoomFinishTime)
    {
        List<int> easy = new List<int>();
        for(int i=1; i <= 3; i++)
        {
            easy.Add(i);
        }
        List<int> medium = new List<int>();
        for(int i=3; i <= 6; i++)
        {
            medium.Add(i);
        }
        List<int> hard = new List<int>();
        for(int i=6; i <= maxEnemyToSpawn; i++)
        {
            hard.Add(i);
        }

        switch(roomNumber)
        {
            case 1:
            if(currentRoomFinishTime < 20)
            {
                //Debug.Log(currentRoomFinishTime + " " + roomNumber);
                int randomEnemyNumber = GetRandomNumber(hard);
                spawnEnemy(randomEnemyNumber);
            }else if(20 < currentRoomFinishTime && currentRoomFinishTime < 40)
            {
                int randomEnemyNumber = GetRandomNumber(medium);
                spawnEnemy(randomEnemyNumber);
            }
            else
            {
                int randomEnemyNumber = GetRandomNumber(easy);
                spawnEnemy(randomEnemyNumber);
            }
            break;
            case 2:
            if(currentRoomFinishTime < 20)
            {
                int randomEnemyNumber = GetRandomNumber(hard);
                spawnEnemy(randomEnemyNumber);
            }else if(20 < currentRoomFinishTime && currentRoomFinishTime < 40)
            {
                int randomEnemyNumber = GetRandomNumber(medium);
                spawnEnemy(randomEnemyNumber);
            }
            else
            {
                int randomEnemyNumber = GetRandomNumber(easy);
                spawnEnemy(randomEnemyNumber);
            }
            break;
            default:
            break;
        }   
    }

    public void spawnEnemy(int totalEnemyToSpawn)
    {
    string dungeonName = sceneName.name;
    GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag(dungeonName + "Spawn" + (roomNumber +  1).ToString());
    spawnedEnemyNum = 0;
    foreach (GameObject spawnerObject in spawnerObjects)
    {
        //Debug.Log("aaa");
        if(spawnedEnemyNum >= totalEnemyToSpawn)
        {
            break;
        }
        else
        {
            Spawner spawner = spawnerObject.GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.SpawnerSpawnEntity(dungeonName, roomNumber);
                spawnedEnemyNum++;
            }
        }
    }
    }


    public int GetRandomNumber(List<int> numbers)
    {
        // Listeden rastgele bir sayı seç
        int randomIndex = UnityEngine.Random.Range(0, numbers.Count); // Listede kaç eleman varsa o aralıkta rastgele bir index seç
        int randomNumber = numbers[randomIndex];

        return randomNumber;
    }
    public void killedBossNarration(int dungeon)
    {
        switch (dungeon)
        {
            case 1:
                newNarration.ReceiveSystemMessage("You killed the boss");
                if (dungeonCount == 4)
                {
                    dungeonCount = 0;
                }
                break;
            case 2:
                newNarration.ReceiveSystemMessage("You killed the boss");
                if (dungeonCount == 4)
                {
                    dungeonCount = 0;
                }
                break;
            case 3:
                newNarration.ReceiveSystemMessage("You killed the boss");
                if (dungeonCount == 4)
                {
                    dungeonCount = 0;
                }
                break;
            case 4:
                newNarration.ReceiveSystemMessage("You killed the boss");
                if (dungeonCount == 4)
                {
                    dungeonCount = 0;   
                }
                break;

        }
    }
}