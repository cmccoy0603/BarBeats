using System.Collections;
using UnityEditor.UI;
using UnityEngine;

// Singleton class for containing all Wwise events
public class AkEventList : MonoBehaviour
{
    public static AkEventList instance;

    public AK.Wwise.Event play_amen_break;
    public AK.Wwise.Event stop_amen_break;

    void Start()
    {
        Debug.Log("(akeventlist) hello world");
        instance = this;

        StartCoroutine(PlayAmenBreak());

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


        Debug.Log("Duplicate AkEventList initialized. Others deleted.");
    }

    private IEnumerator PlayAmenBreak(double seconds = 3)
    {
        Debug.Log("(akeventlist) Playing");
        play_amen_break.Post(gameObject);
        yield return new WaitForSeconds((float)seconds);
        Debug.Log("(akeventlist) Stopping");
        stop_amen_break.Post(gameObject);
    }
}
