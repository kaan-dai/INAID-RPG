using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTask : MonoBehaviour
{
    [SerializeField, TextArea] private string task;

    public string GetTask()
    {
        return $"{task}\n";
    }
}
