using System.Collections.Generic;
using Unity.VisualScripting;

public class BGMTrack
{
    private string internal_name = "Uninitialized";
    private AK.Wwise.Event play_event;
    private AK.Wwise.Event stop_event;

    private static Dictionary<string, BGMTrack> ost = new();

    public static void Init()
    {
        ost.Clear();
        AddToOST("AmenBreak", AkEventList.instance.play_amen_break, AkEventList.instance.stop_amen_break);
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

    public void Play()
    {
        play_event.Post(AkEventList.instance.gameObject);
    }

    public void Stop()
    {
        stop_event.Post(AkEventList.instance.gameObject);
    }

    public static BGMTrack TryGet(string internal_name)
    {
        ost.TryGetValue(internal_name, out var track);
        return track;
    }

    private static void AddToOST(string internal_name, AK.Wwise.Event play_event, AK.Wwise.Event stop_event)
    {
        ost.Add(internal_name, new BGMTrack(internal_name, play_event, stop_event));
    }
}