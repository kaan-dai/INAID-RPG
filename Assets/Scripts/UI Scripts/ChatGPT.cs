using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI
{
    public static class Globals
    {
        public const string MODEL = "gpt-3.5-turbo";
    }

    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private NPCInfo personality;
        [SerializeField] private Facts facts;
        [SerializeField] private EnvInfo envInfo;
        [SerializeField] private NPCStatus npcStatus;
        [SerializeField] private History history;
        [SerializeField] private CurrentTask curTask;

        [SerializeField] private GameObject companion;
        [SerializeField] private GameObject companion2;
        [SerializeField] private GameObject companion3;
        [SerializeField] private RectTransform companionText;

        [SerializeField] private DirectiveHandler directiveHandler;

        [SerializeField] private DungeonMasterInfoCollector infoCollector;

        [SerializeField] private int companionNumber;

        private string recordingContent;

        private bool first = false;

        private bool displayedOnCompanion = false;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private bool canInvoke = true;

        private RectTransform currentMessage;

        private bool activeMessage = false;

        private List<ChatMessage> messages = new List<ChatMessage>();

        private string endString;
        private string curBoss;

        private bool noRecentInvoke = true;

        private void Start()
        {
            SendSystemInfo();
            StartCoroutine(InvokeWithCondition());
            button.onClick.AddListener(SendReply);
        }

        IEnumerator InvokeWithCondition()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                while (!canInvoke)
                {
                    yield return null;
                }

                if (noRecentInvoke) {
                    SendSystemUpdate(); 
                }
                noRecentInvoke = true;
            }
        }

        public void SetRecText(string recordingContentPre)
        {
            recordingContent = recordingContentPre;
            SendReply();
        }

        public void SetCanInvoke(bool a) 
        {
            canInvoke = a;
        }

        private void AppendMessage(ChatMessage message, string content1 = "`")
        {
            if (content1 == "`")
            {
                content1 = message.Content;
            }
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = content1;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            item.GetChild(0).GetChild(0).GetComponent<TypewriterScript>().SetTextAndStartReveal(content1);
            currentMessage = item;
        }

        private bool CheckPauseCondition()
        {
            return currentMessage.GetChild(0).GetChild(0).GetComponent<TypewriterScript>().GetRevealing();
        }

        private async Task WaitWhilePaused()
        {
            while (CheckPauseCondition())
            {
                await Task.Delay(100);
            }
        }

        private async void SendSystemInfo()
        {
            activeMessage = true;
            button.enabled = false;
            inputField.enabled = false;

            var systemPrompt = new ChatMessage()
            {
                Role = "system",
                Content = "This is the setup message. I will give you some information in this message, and in the messages after this one you will respond knowing the information I give you in this message. You only understand English and only answer in English \n" +
                "Here are facts about the world that never change: \n" +
                facts.GetFacts() +
                "Here is your personality, these also never change: \n" +
                personality.GetPersonality() +
                "Your name is " + personality.GetName() + " and you are a " + personality.GetCharClass() +
                "On a scale from 1 to 10, 1 being you dislike me and 10 being you really like me our relation is " + personality.GetRelation() +
                "Here are facts about our current environment, these may change: \n" +
                envInfo.GetEnvInfo() +
                "Here is the history of our travels: \n" +
                history.GetHistory() +
                "And our current task is: \n" +
                curTask.GetTask() +
                "You are in a group of 4 people, you are not alone, if the player addresses you as mutliple people, disregard it, it means the message is being sent to others as well. If the player asks you how many enemies there are, tell them that the small enemies are no bother and that the real deal is the boss, but don't forget that as long as you're not in the Waiting Hall, even if you are not in the boss room there are enemies around, so if the player tells you to attack, attack.\n" +
                "Besides responding to the query by the player, I want you to create a JSON object from the attributes I give you. Never mention the JSON object. I will give you what the attributes mean, and you will fill in the JSON attributes with the information from the \"info text\" and the \"command text\". Always start and end the JSON object with `. Always give the JSON object at the end of your message, never start with the JSON object. If the player is not giving you a command, but simply talking, then only fill the niceness attribute. If the player gives contradictory commands, then ask for elaboration.\r\n\r\n\r\n\r\nIf the player gives you multiple commands within the command text, create multiple JSON objects and put them in the same array.  \r\n\r\nThe \"info text\" lets you know what you have at your disposal and the \"command text\" lets you know what the player wants from you If the player asks for something essential to you, such as your weapon or your clothes, decline, but a health potion is not essential. If the player is asking you to do something, don't narrate the action in your message or respond with the action within asterisks to point out that you are doing the action. Example, if the player is asking you for a health potion, do not say \"*Gives a health potion*\" or \"*Hands over a health potion*\". If the player is being ambiguous in their request, you can ask to confirm. You cannot split the items, for example, a health potion cannot be split between you and anyone else. If the player asks you how you're doing, tell them you're fine taking your relationship into consideration. Never mention the JSON object, only create it and fill it.\r\n\r\n\r\n\r\nThe JSON attributes are as follows: \r\n\r\n1 - command, a string \r\n\r\n2 - niceness, an integer \r\n\r\n3 - commandDoability, a bool\r\n\r\n4 - whatToGive, a string \r\n\r\n\r\n\r\nfill the \"command\" attribute with what command the player is giving you in the command text, it can be the following: goToPlayer, jump, stay, follow, give, use, attack.\r\n\r\n\r\n\r\nExplanations of the commands:\r\n\r\ngoToPlayer means the player wants you to go to them, if you are already right next to the player, you cannot get closer\r\n\r\njump means the player wants you to jump.\r\n\r\nstay means the player wants you to stay where you are, if the player tells you to go away, it also means the player wants you to stay.\r\n\r\nfollow means the player wants you to follow them.\r\n\r\ngive means the player is asking you for an item.\r\n\r\nuse means the player wants you to use an item on yourself, if the player asks you to use a health potion tell them you don't need to because you are doing fine, and decline, if the player asks you to use any other item else, decline.\r\n\r\nattack means the player wants you to attack an enemy, you can't attack if there are no enemies in the scene.\r\n\r\n\r\n\r\nfill the \"niceness\" attribute with how nice the text was towards you, if the message was very rude make niceness \"1\" and if it was very kind make it \"10\". Example, the message \"Can you give me a health potion?\" would be a \"6\", and \"Can you give me a health potion please?\" would be an \"8\".\r\n\r\n\r\n\r\nfill the \"commandDoability\" attribute with whether you can do the command given the information from the info text. An example to make it clearer, if you have no health potions but the player asks you to give them a health potion, commandDoability would be \"false\", if the player asks you for a potion, you have a potion and you are willing to give it to them, then commandDoability is \"true\".  \r\n\r\n\r\n\r\nfill the \"whatToGive\" attribute if the player asks you to give them an item, fill the attribute with the item the player asks from you, it can be \"healthPotion\". \r\n\r\n\r\n\r\nHere is the info text:  " +
                npcStatus.GetNPCStatus() +
                "Occasionally, you will receive messages that start with 'NOT FROM THE PLAYER', this means that the message is only meant as an update on the current status, it is not an actual message from the player, do not refer to these messages when you're talking to the player. and you do not need to respond to these messages\n" +
                "The command texts will follow. "
            };

            messages.Add(systemPrompt);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = Globals.MODEL,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                messages.Add(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
            activeMessage = false;
        }

        private async void SendReply()
        {
            activeMessage = true;
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = (recordingContent == null) ? inputField.text : recordingContent,
            };

            messages.Add(newMessage);

            if (first)
            {
                first = true;
                if (CheckPauseCondition())
                {
                    await WaitWhilePaused();
                }
            }

            AppendMessage(newMessage);

            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = Globals.MODEL,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                message.Content = message.Content.Replace("\\", "");


                int jsonStartIndex = message.Content.IndexOf("{");
                string ab = message.Content;
                if (jsonStartIndex != -1) {
                    if (jsonStartIndex != 0)
                    {
                        if (ab[jsonStartIndex - 1] != '`')
                        {
                            int multiTest = message.Content.IndexOf("[");
                            if (multiTest != -1)
                            {
                                if (ab[multiTest - 1] == '`')
                                {
                                    jsonStartIndex = message.Content.IndexOf("`");
                                }
                                else
                                {
                                    jsonStartIndex = multiTest;
                                }
                            }
                        }
                        else
                        {
                            jsonStartIndex = message.Content.IndexOf("`");
                        }
                    }
                }

                string messagePart = message.Content.Trim();
                
                string jsonPart = null;

                if (jsonStartIndex != -1)
                {
                    messagePart = message.Content.Substring(0, jsonStartIndex).Trim();
                    jsonPart = message.Content.Substring(jsonStartIndex).Trim();
                }
                if (messagePart == "" || messagePart == " ") {
                    messagePart = "...";
                }

                messages.Add(message);

                if (CheckPauseCondition())
                {
                    await WaitWhilePaused();
                }

                if (displayedOnCompanion)
                {
                    companionDisplay(messagePart);
                }

                AppendMessage(message, messagePart);

                if (jsonStartIndex != -1)
                {
                    directiveHandler.readJSON(jsonPart);
                }
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            recordingContent = null;
            button.enabled = true;
            inputField.enabled = true;
            activeMessage = false;
        }

        private async void SendSystemUpdate()
        {
            activeMessage = true;
            button.enabled = false;
            inputField.enabled = false;

            var systemPrompt = new ChatMessage()
            {
                Role = "user",
                Content = "NOT FROM THE PLAYER\n" +
                " New facts about our current environment: \n" +
                endString +
                " And our current task is: \n" +
                curBoss +
                " Here is your current status, the info text: \n" +
                npcStatus.GetNPCStatus() +
                " Here is your updated relationship with the player: " +
                personality.GetRelation()
            };

            Debug.Log(systemPrompt.Content);
            messages.Add(systemPrompt);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = Globals.MODEL,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                messages.Add(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
            activeMessage = false;
        }

        public Button GetButton()
        {
            return button;
        }

        public void SetCompanionDisplay(bool a)
        {
            displayedOnCompanion = a;
        }

        private void companionDisplay(string text)
        {
            int highestOrder = 0;

            if (companionNumber == 0)
            {
                companion = GameObject.Find("MartialHero");
                companion2 = GameObject.Find("Mage");
                companion3 = GameObject.Find("Archer");
            }

            else if (companionNumber == 1)
            {
                companion = GameObject.Find("Mage");
                companion2 = GameObject.Find("Archer");
                companion3 = GameObject.Find("MartialHero");
            }

            else if (companionNumber == 2)
            {
                companion = GameObject.Find("Archer");
                companion2 = GameObject.Find("MartialHero");
                companion3 = GameObject.Find("Mage");
            }

            var item = Instantiate(companionText, companion.transform.GetChild(0));
            item.transform.localPosition = Vector3.zero;
            highestOrder = Mathf.Max(companion.transform.GetChild(0).GetComponent<Canvas>().sortingOrder, Mathf.Max(companion2.transform.GetChild(0).GetComponent<Canvas>().sortingOrder, companion3.transform.GetChild(0).GetComponent<Canvas>().sortingOrder)); ;
            companion.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = highestOrder + 1;
            item.GetChild(0).GetChild(0).GetComponent<TypewriterScript>().SetTextAndStartReveal(text);
            displayedOnCompanion = false;
            Destroy(item.gameObject, 7f);
        }

        void Update() {
            infoCollector = GameObject.Find("Player").GetComponent<DungeonMasterInfoCollector>();
            if (companionNumber == 0)
            {
                infoCollector.gpt1 = this;
            }
            else if (companionNumber == 1)
            {
                infoCollector.gpt2 = this;
            }
            else if (companionNumber == 2) 
            {
                infoCollector.gpt3 = this;
            }
        }

        public void SetActiveMessage(bool a) {
            activeMessage = a;
        }

        public bool GetActiveMessage() {
            return activeMessage;
        }

        public void SetEnvironmentInfo(string a = "", string b = "") {
            endString = a;
            curBoss = b;
            noRecentInvoke = false;
            SendSystemUpdate();
            Debug.Log("UPDATED WITH INFO: " + a + "\n " + b);
        }

        public void PotionUpdate() {
            SendSystemUpdate();
        }
    }
}