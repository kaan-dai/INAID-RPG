using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour
{
    [SerializeField, TextArea] private string history;

    public string GetHistory()
    {
        return $"{history}\n";
    }
}
