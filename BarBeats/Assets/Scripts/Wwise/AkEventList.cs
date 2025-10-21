using UnityEngine;

// Singleton class for containing all Wwise events
public class AkEventList : MonoBehaviour
{
    public static AkEventList instance;

    public AK.Wwise.Event play_amen_break;

    void Start()
    {
        instance = this;

        // Destroy all other instances of AkEventList
        if (FindObjectsByType<AkEventList>(FindObjectsSortMode.None).Length <= 1) return;

        foreach (var obj in FindObjectsByType<AkEventList>(FindObjectsSortMode.None))
        {
            if (obj == this) continue;

            Destroy(obj.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
