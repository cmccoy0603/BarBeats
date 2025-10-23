using System.Collections;
using UnityEngine;

// Singleton class for containing all Wwise events
public class AkEventList : MonoBehaviour
{
    public static AkEventList instance;

    public AK.Wwise.Event play_amen_break;
    public AK.Wwise.Event stop_amen_break;

    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        // Destroy all other instances of AkEventList
        if (FindObjectsByType<AkEventList>(FindObjectsSortMode.None).Length <= 1)
        {
            Debug.Log("AkEventList initialized.");
            return;
        }

        foreach (var obj in FindObjectsByType<AkEventList>(FindObjectsSortMode.None))
        {
            if (obj == this) continue;

            Destroy(obj.gameObject);
        }
    }

    void Update()
    {
        int playback_ms = BGMTrack.GetProgress();
        // Debug.Log("" + BGMTrack.wwise_current_playing_id + " " + playback_ms);
    }

    public static bool IsNull()
    {
        return instance == null;
    }
}
