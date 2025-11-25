using System;
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

    private static DateTime most_recent_window_open_time;

    void Start()
    {
        ak_flags.value = (uint)AkCallbackType.AK_EnableGetSourcePlayPosition;
        gameObject_id = AkUnitySoundEngine.GetAkGameObjectID(gameObject);
        StartCoroutine(GuaranteeInitTrack());
        most_recent_window_open_time = DateTime.Now.AddMilliseconds(-300);
    }

    public void SwitchToTrack(string internal_name)
    {
        Stop();

        current_track = BGMTrack.TryGet(internal_name);
        if (current_track == null)
        {
            Debug.LogWarning($"BGM track {internal_name} does not exist");
            return;
        }

        print($"Starting BGM track {internal_name} using event {current_track.play_event}");

        Play();
    }

    // public (uint, int) GetPlayheadProgressMilliseconds()
    // {
    //     uint playing_id = GetPlayingID();

    //     if (IsInvalidPlayingID(playing_id)) return (AkUnitySoundEngine.AK_INVALID_PLAYING_ID, 0);

    //     _ = AkUnitySoundEngine.GetSourcePlayPosition(playing_id, out int playback_ms);
    //     return (playing_id, playback_ms);
    // }

    public float GetRhythmSyncScore()
    {
        // (uint playing_id, int progress_ms) = GetPlayheadProgressMilliseconds();
        // if (IsInvalidPlayingID(playing_id)) return 0;

        // const double strictness = 0;
        // const int height = 1;
        // return (float)math.max(
        //     0,
        //     (
        //         height
        //         + strictness
        //     )
        //     * math.cos(
        //         math.TAU
        //         * (progress_ms + 280)
        //         / MS_PER_PULSE
        //     ) - strictness
        // );

        return (DateTime.Now.Subtract(most_recent_window_open_time).TotalMilliseconds < 100) ? 1 : 0;
    }

    public void LetMeKnowEarlyMyAttackWasOnNextBeat(float rhythm_score)
    {
        // StartCoroutine(WaitForAndPrintAttackEarlinessData(rhythm_score));
    }

    public void BeatWindow()
    {
        AkEventList.instance.play_bonk.Post(gameObject);
        StartCoroutine(OpenBeatWindowAfterDelay(AkEventList.audio_device_latency_ms));
    }

    private IEnumerator OpenBeatWindowAfterDelay(int ms)
    {
        yield return new WaitForSeconds(ms / 1000f);
        most_recent_window_open_time = DateTime.Now;

    }

    // private IEnumerator WaitForAndPrintAttackEarlinessData(float rhythm_score)
    // {
    //     DateTime start_time = DateTime.Now;
    //     while (GetRhythmSyncScore() < 0.97)
    //     {
    //         yield return null;
    //     }
    //     double delta_ms = DateTime.Now.Subtract(start_time).TotalMilliseconds;
    //     double delta_beats = delta_ms / MS_PER_PULSE;
    //     //print($"<color=cyan>{delta_beats}</color> beats too early (<color=yellow>{(int)delta_ms}</color> ms). <color=red>RS: {rhythm_score}</color>");
    // }

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
        while (AkEventList.NullCheck())
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
