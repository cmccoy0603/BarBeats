using System.Collections;
using UnityEngine;

// Component that plays a BGM track on its gameObject.
// No other events should be posted to this gameObject.
public class MusicPlayer : MonoBehaviour
{
    public string init_track_name = "AmenBreak";
    private BGMTrack current_track = null;
    private ulong gameObject_id;
    private static AK.Wwise.CallbackFlags ak_flags = new();

    void Start()
    {
        ak_flags.value = (uint)AkCallbackType.AK_EnableGetSourcePlayPosition;
        gameObject_id = AkUnitySoundEngine.GetAkGameObjectID(gameObject);
        DontDestroyOnLoad(gameObject);
        StartCoroutine(GuaranteeInitTrack());

    }

    void Update()
    {
        Debug.Log("" + GetPlayheadProgressMilliseconds());
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

    public int GetPlayheadProgressMilliseconds()
    {
        uint num_ids = 1; // We expect at least one playing ID
        uint[] playing_ids = new uint[1]; // Array to store the result
        AkUnitySoundEngine.GetPlayingIDsFromGameObject(gameObject_id, ref num_ids, playing_ids);

        if (num_ids <= 0 || playing_ids[0] == AkUnitySoundEngine.AK_INVALID_PLAYING_ID)
        {
            Debug.Log("no playing IDs found");
            return 0;
        }
        Debug.Log($"{num_ids} playing IDs found");
        AkUnitySoundEngine.GetSourcePlayPosition(playing_ids[0], out int playback_ms);
        return playback_ms;
    }

    private void Play()
    {
        current_track?.play_event.Post(gameObject, ak_flags, Noop);
    }

    private void Stop()
    {
        current_track?.stop_event.Post(gameObject);
    }
    private IEnumerator GuaranteeInitTrack()
    {
        while (AkEventList.IsNull())
        {
            yield return null;
        }
        SwitchToTrack(init_track_name);
    }

    private void Noop(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        return;
    }
}
