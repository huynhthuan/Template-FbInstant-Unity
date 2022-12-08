using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public bool isDontDestroy;

    private void Awake()
    {
        instance = this as T;
        if (isDontDestroy)
        {
            DontDestroyOnLoad(this);
        }
    }

    public static bool Exists()
    {
        return (instance != null);
    }
}