using UnityEngine;
using UnityEngine.UI;

public class ToggleCanvas : MonoBehaviour
{
    public Canvas canvasToToggle;
    public Canvas canvasSwitch1;
    public Canvas canvasSwitch2;
    public Canvas pauseCanvas;
    [SerializeField] private WhisperCaller whisperCaller;
    [SerializeField] private int chatChooseNumber;
    private Button button;

    private void Start()
    {
        canvasToToggle.gameObject.GetComponent<Canvas>().enabled = false;
        button = GetComponent<Button>();
    }

    public void Update()
    {
        if (whisperCaller.GetBoolForToggle() && whisperCaller.GetWhisper() != chatChooseNumber)
        {
            button.interactable = false;
        }
        else {
            button.interactable = true;
        }
    }

    public void ToggleCanvasVisibility()
    {
        if (chatChooseNumber != -1)
        {
            whisperCaller.SetWhisper(chatChooseNumber+1);
        }
        canvasToToggle.gameObject.GetComponent<Canvas>().enabled = (!canvasToToggle.gameObject.GetComponent<Canvas>().enabled);
        if (canvasSwitch1.gameObject.GetComponent<Canvas>().enabled)
        {
            canvasSwitch1.gameObject.GetComponent<Canvas>().enabled = false;
        }
        if (canvasSwitch2.gameObject.GetComponent<Canvas>().enabled)
        {
            canvasSwitch2.gameObject.GetComponent<Canvas>().enabled = false;
        }
        if (pauseCanvas.gameObject.GetComponent<Canvas>().enabled)
        {
            pauseCanvas.gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
