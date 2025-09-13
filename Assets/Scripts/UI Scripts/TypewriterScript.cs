using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterScript : MonoBehaviour
{
    public float revealSpeed = 0.05f; // Speed at which letters are revealed
    [SerializeField] private Text textComponent;
    private string fullText;
    private string currentText = "";
    private bool isRevealing = false;

    public void SetTextAndStartReveal(string newText)
    {
        if (!isRevealing)
        {
            isRevealing = true;
            fullText = newText;
            textComponent.text = "";
            StartCoroutine(RevealText());
        }
    }

    private IEnumerator RevealText()
    {
        float startTime = Time.realtimeSinceStartup;
        int currentIndex = 0;

        while (currentIndex < fullText.Length)
        {
            if (Time.realtimeSinceStartup - startTime >= revealSpeed)
            {
                currentText += fullText[currentIndex];
                textComponent.text = currentText;
                currentIndex++;
                startTime = Time.realtimeSinceStartup; // Reset the start time
            }
            yield return null;
        }
        isRevealing = false;
    }

    public bool GetRevealing() {
        return isRevealing;
    }

}


