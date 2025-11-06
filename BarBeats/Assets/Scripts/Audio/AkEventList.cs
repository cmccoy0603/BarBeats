using System.Collections;
using UnityEngine;

// Singleton class for containing all Wwise events
public class AkEventList : MonoBehaviour
{
    public static AkEventList instance;

    public AK.Wwise.Event play_amen_break;
    public AK.Wwise.Event stop_amen_break;
    public AK.Wwise.Event play_medley_bgm;
    public AK.Wwise.Event stop_medley_bgm;

    public AK.Wwise.State medley_bgm_choice_none;
    public AK.Wwise.State medley_bgm_choice_march;
    public AK.Wwise.State medley_bgm_choice_western;

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

    public static bool NullCheck()
    {
        bool r = instance == null;
        if (r)
        {
            Debug.LogWarning("AkEventList is null");
        }
        return r;
    }
}
