using UnityEngine;

public class EnvInfo : MonoBehaviour
{
    [SerializeField, TextArea] private string envInfo;

    public string GetEnvInfo()
    {
        return $"{envInfo}\n";
    }
}
