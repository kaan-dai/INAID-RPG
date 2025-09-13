using OpenAI;
using UnityEngine;
using UnityEngine.UI;

public class Whisper : MonoBehaviour
{
    [SerializeField] private Button recordButton;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image circleProgressBar;
    [SerializeField] private Dropdown dropdown;
    [SerializeField] public ChatGPT chatGpt;
    [SerializeField] public ChatGPT chatGpt2;
    [SerializeField] public ChatGPT chatGpt3;
    [SerializeField] private CircleSelector circleSelector;
    [SerializeField] private WhisperCaller whisperCaller;

    private readonly string fileName = "output.wav";
    private readonly int duration = 4;

    private bool discarded = false;

    private AudioClip clip;
    private bool isRecording = false;
    private float time;
    private OpenAIApi openai = new OpenAIApi();
    private bool multi = false;

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
#else
        foreach (var device in Microphone.devices)
        {
            dropdown.options.Add(new Dropdown.OptionData(device));
        }
        recordButton.onClick.AddListener(StartRecording);
        dropdown.onValueChanged.AddListener(ChangeMicrophone);

        var index = PlayerPrefs.GetInt("user-mic-device-index");
        dropdown.SetValueWithoutNotify(index);
#endif
    }

    private void ChangeMicrophone(int index)
    {
        PlayerPrefs.SetInt("user-mic-device-index", index);
    }

    public void StartRecording()
    {
        if (!isRecording)
        {
            circleSelector.SetKeyEnabled(false);
            time = 0;
            var index = PlayerPrefs.GetInt("user-mic-device-index");
#if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            isRecording = true;
#endif
        }
        else
        {
            EndRecording();
            isRecording = false;
        }

    }

    private async void EndRecording()
    {
#if !UNITY_WEBGL
        Microphone.End(null);
#endif
        if (!discarded) {
            byte[] data = SaveWav.Save(fileName, clip);
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            if (multi)
            {
                chatGpt.SetRecText(res.Text);
                chatGpt2.SetRecText(res.Text);
                chatGpt3.SetRecText(res.Text);
                multi = false;
            }
            else {
                chatGpt.SetRecText(res.Text);
            }
            
        }
        discarded = false;
        circleProgressBar.fillAmount = 0;
        progressBar.fillAmount = 0;
        recordButton.interactable = true;
        circleSelector.SetKeyEnabled(true);
    }

    private void Update()
    {
        if (isRecording)
        {
            time += Time.unscaledDeltaTime;

            circleProgressBar.fillAmount = time / duration;
            progressBar.fillAmount = time / duration;

            if (Input.GetKeyDown(KeyCode.B)) {
                time = 0;
                recordButton.interactable = false;
                isRecording = false;
                whisperCaller.SetRecordingComplete(true);
                discarded = true;
                EndRecording();
            }

            if (time >= duration)
            {
                time = 0;
                recordButton.interactable = false;
                isRecording = false;
                whisperCaller.SetRecordingComplete(true);
                EndRecording();
            }
        }
    }

    public Button GetRecButton()
    {
        return recordButton;
    }

    public ChatGPT GetChatGPT()
    {
        return chatGpt;
    }

    public void SetMulti() {
        multi = true;
    }
}

