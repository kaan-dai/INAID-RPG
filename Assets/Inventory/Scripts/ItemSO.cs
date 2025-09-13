using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public float amountToChangeStat;
    private GameObject player; 


    public void UseItem()
    {
        if(statToChange == StatToChange.health)
        {
            player = GameObject.Find("Player");

            if (player.GetComponent<ScriptForPlayerRest>() != null)
            {
                ScriptForPlayerRest script = player.GetComponent<ScriptForPlayerRest>();
                script.RestoreHealth(amountToChangeStat);
                
            }
            if (player.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = player.GetComponent<NewScriptForPlayerSnow>();
                script.RestoreHealth(amountToChangeStat);
            }
        }
        else if(statToChange == StatToChange.attack)
        {
            player = GameObject.Find("Player");
            
            if (player.GetComponent<ScriptForPlayerRest>() != null)
            {
                ScriptForPlayerRest script = player.GetComponent<ScriptForPlayerRest>();
                script.IncreaseDamage(amountToChangeStat);
            }
            if (player.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = player.GetComponent<NewScriptForPlayerSnow>();
                script.IncreaseDamage(amountToChangeStat);
            }
        }
        else if(statToChange == StatToChange.defence)
        {
            player = GameObject.Find("Player");

           if (player.GetComponent<ScriptForPlayerRest>() != null)
            {
                ScriptForPlayerRest script = player.GetComponent<ScriptForPlayerRest>();
                script.IncreaseDefense(amountToChangeStat);
            }
            if (player.GetComponent<NewScriptForPlayerSnow>() != null)
            {
                NewScriptForPlayerSnow script = player.GetComponent<NewScriptForPlayerSnow>();
                script.IncreaseDefense(amountToChangeStat);
            }
        }
        else if(statToChange == StatToChange.none) // Use this for key item maybe?
        {
            Debug.Log("none item used.");
        }
    }


    public enum StatToChange
    {
        none,
        health,
        attack,
        defence
    };
}
