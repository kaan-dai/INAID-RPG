using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class CircleSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] sections;
    [SerializeField] private Canvas circleWhole;
    [SerializeField] private WhisperCaller whisperCaller;
    [SerializeField] private InputField inputField1;
    [SerializeField] private InputField inputField2;
    [SerializeField] private InputField inputField3;
    private int selectedSection = -1;
    private Vector3 lastMousePosition;
    private bool circleAble = true;
    private bool keyEnabled = true;

    void Start()
    {
        circleWhole.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == inputField1.gameObject || EventSystem.current.currentSelectedGameObject == inputField2.gameObject || EventSystem.current.currentSelectedGameObject == inputField3.gameObject)
        {
            keyEnabled = false;
        }
        else
        {
            keyEnabled = true;
        }

        if (keyEnabled)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                whisperCaller.SetSpeakable(false);
                Time.timeScale = 0.1f;
                DisplayCircle(true);

                Vector3 currentMousePosition = Input.mousePosition;
                if (currentMousePosition != lastMousePosition)
                {
                    Vector3 difference = currentMousePosition - lastMousePosition;
                    if (difference.magnitude > 20)
                    {
                        Vector2 direction = currentMousePosition - lastMousePosition;
                        lastMousePosition = currentMousePosition;
                        if (circleAble)
                        {
                            DetectDirectionGroup(direction);
                        }

                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                whisperCaller.SetWhisper(selectedSection);
                DisplayCircle(false);
                Time.timeScale = 1;
                whisperCaller.SetSpeakable(true);
            }
        }
    }

    private bool IsMouseOverSection(Vector2 mousePos, int index)
    {
        return false;
    }

    public void DisplayCircle(bool show)
    {
        circleWhole.gameObject.SetActive(show);
    }

    private void HighlightSection(int index)
    {
        sections[index].transform.GetChild(0).transform.localScale = new Vector3(1.08f, 1.08f, 1);
        Transform objectTransform = sections[index].transform;
        objectTransform.SetAsLastSibling();
    }

    private void UnhighlightSection(int index)
    {
        sections[index].transform.GetChild(0).transform.localScale = Vector3.one;
    }

    public int GetSelection()
    {
        return selectedSection;
    }

    private void DetectDirectionGroup(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if ((angle > 95 && angle < 175))
        {
            selectedSection = 0;
            HighlightSection(0);
            UnhighlightSection(2);
            UnhighlightSection(1);
            UnhighlightSection(3);
        }
        else if (angle < -5 && angle > -85)
        {
            selectedSection = 2;
            HighlightSection(2);
            UnhighlightSection(1);
            UnhighlightSection(0);
            UnhighlightSection(3);
        }
        else if (angle < -95 && angle > -175)
        {
            selectedSection = 3;
            HighlightSection(3);
            UnhighlightSection(1);
            UnhighlightSection(0);
            UnhighlightSection(2);
        }
        else if ((angle < 85 && angle > 5))
        {
            selectedSection = 1;
            HighlightSection(1);
            UnhighlightSection(0);
            UnhighlightSection(2);
            UnhighlightSection(3);
        }


    }

    public void SetCircleAble(bool a)
    {
        circleAble = a;
        UnhighlightSection(0);
        UnhighlightSection(1);
        UnhighlightSection(2);
        UnhighlightSection(3);
    }

    public void SetKeyEnabled(bool a)
    {
        keyEnabled = a;
    }
}
