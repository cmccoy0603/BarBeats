using UnityEngine;


public class BGMTrack
{
    public string song_name = "Example Song";
    public string internal_song_name = "ExampleSong";
    public AK.Wwise.Event play_event;
    public AK.Wwise.Event stop_event;

    public BGMTrack(
        string song_name,
        string internal_song_name,
        AK.Wwise.Event play_event,
        AK.Wwise.Event stop_event
    )
    {
        this.song_name = song_name;
        this.internal_song_name = internal_song_name;
        this.play_event = play_event;
        this.stop_event = stop_event;
    }
}