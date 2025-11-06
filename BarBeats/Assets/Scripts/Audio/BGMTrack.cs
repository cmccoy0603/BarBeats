using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGMTrack
{
    private string internal_name = "Uninitialized";
    public AK.Wwise.Event play_event { get; private set; }
    public AK.Wwise.Event stop_event { get; private set; }

    private static Dictionary<string, BGMTrack> ost = new();
    public static uint wwise_current_playing_id { get; private set; } = AkUnitySoundEngine.AK_INVALID_PLAYING_ID;

    private static void InitIfNecessary()
    {
        if (AkEventList.NullCheck()) return;
        if (ost.ContainsKey("AmenBreak")) return;
        AddToOST("AmenBreak", AkEventList.instance.play_amen_break, AkEventList.instance.stop_amen_break);
        AddToOST("MedleyBGM", AkEventList.instance.play_medley_bgm, AkEventList.instance.stop_medley_bgm);
    }

    public BGMTrack(
        string internal_name,
        AK.Wwise.Event play_event,
        AK.Wwise.Event stop_event
    )
    {
        this.internal_name = internal_name;
        this.play_event = play_event;
        this.stop_event = stop_event;
    }

    public static BGMTrack TryGet(string internal_name)
    {
        InitIfNecessary();
        ost.TryGetValue(internal_name, out var track);
        if (track == null)
        {
            Debug.LogWarning($"BGM Track {internal_name} does not exist");
        }
        return track;
    }

    private static void AddToOST(string internal_name, AK.Wwise.Event play_event, AK.Wwise.Event stop_event)
    {
        ost.Add(internal_name, new BGMTrack(internal_name, play_event, stop_event));
    }

    public static int GetProgress()
    {
        AkUnitySoundEngine.GetSourcePlayPosition(wwise_current_playing_id, out int playback_ms);
        return playback_ms;
    }
}