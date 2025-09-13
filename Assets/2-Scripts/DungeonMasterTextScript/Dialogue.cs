using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace OpenAI
{
    public class Dialogue : MonoBehaviour
    {
        private Dictionary<string, string> sceneNarrations = new Dictionary<string, string>()
        {
            {"WaitingHallScene", "You are in the Waiting Hall."},
            {"RockyScene", "You enter the Rocky Dungeon(Rocky, cold)."},
            {"CrystalScene", "You enter the Crystal Dungeon(Colorful)."},
            {"SnowScene", "You enter the Snow Dungeon(Very cold, very white)."},
            {"DesertScene", "You enter the Desert Dungeon(Very hot, sun in the sky)."}
        };
        public TextMeshProUGUI textComponent;
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You are a dungeon master for an AI driven 2D RPG game. Game will send you interaction logs and you will narrate them. Don't say anything about the player. Don't get out of character.\nThe outputs must be in two to three sentences, it mustn't be one sentence. Make short but descriptive sentences. Don't add any additional details that might destroy the story continuity. \nDON'T ANSWER FOR THE FIRST TIME. Don't get out of character. Don't say that you are an AI. The outpust must be one paragraph";

        private void Start()
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneNarrations.ContainsKey(currentScene))
            {
                // If it does, use it
                ReceiveSystemMessage(sceneNarrations[currentScene]);
            }
            else
            {
                // Otherwise, use a default message
                ReceiveSystemMessage("You enter an unknown location.");
            }
            
        }
        public async void ReceiveSystemMessage(string systemMessage)
        {

            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = systemMessage
            };
            
            if (messages.Count == 0) newMessage.Content = prompt + "\n" + systemMessage; 
            
            messages.Add(newMessage);
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                textComponent.text = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }

    }
}