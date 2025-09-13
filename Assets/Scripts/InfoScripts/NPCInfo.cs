using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public enum CharClass
{
    Mage,
    Archer,
    Samurai,
    Warrior
}

public class NPCInfo : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private CharClass npcClass;
    [SerializeField, TextArea] private string personality;
    [SerializeField] private int relationship;
    private List<int> relationMarkers = new List<int>();
    private Scene currentScene;

    void Start() {
        currentScene = SceneManager.GetActiveScene();
    }

    public string GetPersonality()
    {
        return $"{personality}\n";
    }

    public string GetRelation()
    {
        return $"{relationship.ToString()}";
    }

    public void SetRelation(int a) {
        relationMarkers.Add(a);
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene != currentScene)
        {
            currentScene = activeScene;
            double average = relationMarkers.Average();
            if (average > relationship) 
            {
                relationship++;
            }
            if (average < relationship)
            {
                relationship--;
            }
        }
    }

    public string GetCharClass()
    {
        return $"{npcClass.ToString()}";
    }

    public string GetName()
    {
        return $"{npcName}";
    }
}