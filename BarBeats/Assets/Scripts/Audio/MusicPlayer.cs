using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public string init_track_name = "AmenBreak";
    private BGMTrack current_track = null;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(GuaranteeInitTrack());
    }

    private IEnumerator GuaranteeInitTrack()
    {
        while (current_track == null)
        {
            SwitchToTrack(init_track_name);
            yield return null;
        }
    }

    public void SwitchToTrack(string internal_name)
    {
        Stop();

        current_track = BGMTrack.TryGet(internal_name);
        if (current_track == null)
        {
            Debug.Log("" + internal_name + " does not exist");
        }
        if (current_track == null) return;

        Play();
    }

    private void Play()
    {
        current_track?.play_event.Post(gameObject);
    }

    private void Stop()
    {
        current_track?.stop_event.Post(gameObject);
    }
}
