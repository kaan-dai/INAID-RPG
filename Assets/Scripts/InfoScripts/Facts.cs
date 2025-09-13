using UnityEngine;

public class Facts : MonoBehaviour
{
    [SerializeField, TextArea] private string facts;

    public string GetFacts() {
        return $"{facts}\n";
    }
}
