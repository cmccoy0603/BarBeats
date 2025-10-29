using System.Collections;
using System.ComponentModel;
using Unity.Mathematics;
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
        StartCoroutine(GuaranteeInitTrack());
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

    public (uint, int) GetPlayheadProgressMilliseconds()
    {
        uint playing_id = GetPlayingID();

        if (IsInvalidPlayingID(playing_id)) return (AkUnitySoundEngine.AK_INVALID_PLAYING_ID, 0);

        AkUnitySoundEngine.GetSourcePlayPosition(playing_id, out int playback_ms);
        return (playing_id, playback_ms);
    }

    public float GetRhythmSyncScore()
    {
        (uint playing_id, int progress_ms) = GetPlayheadProgressMilliseconds();
        if (IsInvalidPlayingID(playing_id)) return 0;

        const double tempo = 137;
        const double ms_per_pulse = 60000.0 / tempo;
        const double strictness = 0;
        const int height = 1;
        return (float)math.max(
            0,
            (
                height
                + strictness
            )
            * math.cos(
                math.TAU
                * progress_ms
                / ms_per_pulse
            ) - strictness
        );
    }

    private uint GetPlayingID()
    {
        uint num_ids = 1; // We expect at least one playing ID
        uint[] playing_ids = new uint[1]; // Array to store the result
        AkUnitySoundEngine.GetPlayingIDsFromGameObject(gameObject_id, ref num_ids, playing_ids);

        if (num_ids <= 0)
        {
            return AkUnitySoundEngine.AK_INVALID_PLAYING_ID;
        }

        return playing_ids[0];
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

    private static bool IsInvalidPlayingID(uint id)
    {
        return id == AkUnitySoundEngine.AK_INVALID_PLAYING_ID;
    }
}
