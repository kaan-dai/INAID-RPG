using UnityEngine;

public class Pause : MonoBehaviour
{
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public Canvas pauseScreen;
    [SerializeField] private CircleSelector ccc;
    [SerializeField] private WhisperCaller whisperCaller;

    public void TogglePause()
    {
        if (canvas1.gameObject.GetComponent<Canvas>().enabled || canvas2.gameObject.GetComponent<Canvas>().enabled || canvas3.gameObject.GetComponent<Canvas>().enabled || pauseScreen.gameObject.GetComponent<Canvas>().enabled)
        {
            ccc.SetKeyEnabled(false);
            ccc.DisplayCircle(false);
            whisperCaller.SetCircleActive(false);

            PauseGame();

        }
        else
        {
            ResumeGame();
            if (Input.GetKey(KeyCode.V))
            {
                whisperCaller.SetCircleActive(true);
            }
            whisperCaller.SetCircleActiveBool(true);
            ccc.SetKeyEnabled(true);
        }
    }

    void Update()
    {

    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
