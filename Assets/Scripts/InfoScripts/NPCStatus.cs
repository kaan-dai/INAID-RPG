using UnityEngine;
using OpenAI;

public class NPCStatus : MonoBehaviour
{
    [SerializeField, TextArea] private string npcStatus;
    [SerializeField] public int potionAmount = 1;
    [SerializeField] private GameObject companion;
    [SerializeField] private GameObject player;
    [SerializeField] private int companionNumber;
    [SerializeField] private ChatGPT chatGpt;

    public string GetNPCStatus()
    {
        if (companionNumber == 0)
        {
            companion = GameObject.Find("MartialHero");
        }
        else if (companionNumber == 1)
        {
            companion = GameObject.Find("Mage");
        }
        else if (companionNumber == 2) {
            companion = GameObject.Find("Archer");
        }

        player = GameObject.Find("Player");

        float distance = Vector3.Distance(companion.transform.position, player.transform.position);

        string distanceString = "close to";
        if (distance <= 0.75) {
            distanceString = "right next to";
        }
        else if (distance <= 2.0) 
        {
            distanceString = "close to";
        }
        else if (distance <= 5.0)
        {
            distanceString = "a bit far away from";
        }
        else if (distance <= 10.0)
        {
            distanceString = "far away from";
        }
        return $"You have {potionAmount} health potions and you are {distanceString} the player\n";
    }

    public void UsedAHealthPotion() {
        potionAmount--;
        chatGpt.PotionUpdate();
        companion.GetComponent<CompanionAI>().DropPotion();
        Debug.Log(potionAmount);
    }
}
