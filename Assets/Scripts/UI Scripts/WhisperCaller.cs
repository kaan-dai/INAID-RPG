using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WhisperCaller : MonoBehaviour
{
    [SerializeField] private Whisper[] whispers;
    [SerializeField] private CircleSelector ccc;
    [SerializeField] private Canvas circleCanvas;
    [SerializeField] private InputField inputField1;
    [SerializeField] private InputField inputField2;
    [SerializeField] private InputField inputField3;
    private int selectedSection = 0;
    private bool speakable = true;
    private bool activeCircle = true;
    private bool recordingComplete = false;
    private bool boolForToggle = false;

    void Start()
    {
        circleCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == inputField1.gameObject || EventSystem.current.currentSelectedGameObject == inputField2.gameObject || EventSystem.current.currentSelectedGameObject == inputField3.gameObject)
        {
            speakable = false;
        }
        else
        {
            speakable = true;
        }

        foreach (var whisper in whispers)
        {
            whisper.chatGpt.SetCanInvoke(speakable);
        }


        if (speakable)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                whispers[0].chatGpt.SetActiveMessage(true);

                boolForToggle = true;
                recordingComplete = false;
                whispers[0].GetRecButton().interactable = false;
                whispers[0].GetChatGPT().GetButton().interactable = false;
                whispers[1].GetRecButton().interactable = false;
                whispers[1].GetChatGPT().GetButton().interactable = false;
                whispers[2].GetRecButton().interactable = false;
                whispers[2].GetChatGPT().GetButton().interactable = false;
                ccc.SetCircleAble(false);
                ccc.SetKeyEnabled(false);
                if (activeCircle)
                {
                    circleCanvas.gameObject.SetActive(true);
                }

                if (selectedSection == -1)
                {
                    whispers[0].GetChatGPT().SetCompanionDisplay(true);
                    whispers[0].StartRecording();
                    whispers[0].SetMulti();
                    whispers[1].GetChatGPT().SetCompanionDisplay(true);
                    whispers[2].GetChatGPT().SetCompanionDisplay(true);
                }
                else {
                    whispers[selectedSection].GetChatGPT().SetCompanionDisplay(true);
                    whispers[selectedSection].StartRecording();
                }

                
            }

            else if ((Input.GetKeyUp(KeyCode.V) && !recordingComplete) || recordingComplete)
            {
                if (!recordingComplete)
                {
                    if (selectedSection == -1) {
                        whispers[0].StartRecording();
                    } else 
                    { 
                        whispers[selectedSection].StartRecording();
                    }
                }
                circleCanvas.gameObject.SetActive(false);
                whispers[0].GetRecButton().interactable = true;
                whispers[0].GetChatGPT().GetButton().interactable = true;
                whispers[1].GetRecButton().interactable = true;
                whispers[1].GetChatGPT().GetButton().interactable = true;
                whispers[2].GetRecButton().interactable = true;
                whispers[2].GetChatGPT().GetButton().interactable = true;
                ccc.SetCircleAble(true);
                ccc.SetKeyEnabled(true);
                boolForToggle = false;
            }
        }
    }

    public void SetWhisper(int index)
    {
        selectedSection = index - 1;
    }
    public int GetWhisper()
    {
        return selectedSection;
    }
    public void SetSpeakable(bool a)
    {
        speakable = a;
    }

    public bool GetSpeakable()
    {
        return speakable;
    }

    public void SetCircleActive(bool a)
    {
        circleCanvas.gameObject.SetActive(a);
        activeCircle = a;
    }

    public void SetCircleActiveBool(bool a)
    {
        activeCircle = a;
    }

    public void SetRecordingComplete(bool a)
    {
        recordingComplete = a;
    }

    public bool GetBoolForToggle()
    {
        return boolForToggle;
    }
}


