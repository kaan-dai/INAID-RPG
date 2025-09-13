using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

public class Command
{
    public string command;
    public int niceness;
    public bool commandDoability;
    public string whatToGive;
}

public class DirectiveHandler : MonoBehaviour
{
    [SerializeField] NPCInfo npcInfo;
    [SerializeField] NPCStatus npcStatus;
    [SerializeField] int companionNumber;
    [SerializeField] private CompanionAI companionAi;

    public void readJSON(string jsonObj)
    {
        if (jsonObj != null) {
            List<Command> commands = new List<Command>();
            string[] targets = new[] { "`", "json", "[", "]" };
            string a = CleanString(jsonObj, targets);

            List<string> tokens = Tokenize(a);

            foreach (string t in tokens)
            {
                Command command = JsonUtility.FromJson<Command>(t);
                commands.Add(command);
            }

            callDirectives(commands);
        }
    }

    private static string CleanString(string input, string[] targets)
    {
        StringBuilder sb = new StringBuilder(input);
        foreach (var target in targets)
        {
            sb.Replace(target, string.Empty);
        }
        return sb.ToString();
    }

    private List<string> Tokenize(string input)
    {
        string pattern = @"\{[^}]+\}";
        MatchCollection matches = Regex.Matches(input, pattern);

        List<string> tokens = new List<string>();

        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }

        return tokens;
    }

    private void callDirectives(List<Command> commands)
    {
        if (companionNumber == 0)
        {
            companionAi = GameObject.Find("MartialHero").GetComponent<CompanionAI>();
        }
        else if (companionNumber == 1)
        {
            companionAi = GameObject.Find("Mage").GetComponent<CompanionAI>();
        }
        else if (companionNumber == 2)
        {
            companionAi = GameObject.Find("Archer").GetComponent<CompanionAI>();
        }
        foreach (Command c in commands)
        {
            npcInfo.SetRelation(c.niceness);
            Debug.Log(c.niceness);
            Debug.Log(c.command);
            if (c.commandDoability) {
                switch (c.command)
                {
                    case "goToPlayer":
                        companionAi.Follow();
                        break;
                    case "jump":
                        companionAi.Jump();
                        break;
                    case "stay":
                        companionAi.Unfollow();
                        break;
                    case "follow":
                        companionAi.Follow();
                        break;
                    case "give":
                        npcStatus.UsedAHealthPotion();
                        break;
                    case "attack":
                        companionAi.AttackNow();
                        break;
                    case null:
                        //NO DIRECTIVE ONLY NICENESS
                        break;
                    default:
                        Debug.LogError("Unknown state");
                        break;
                }
            }
        }
    }
}
