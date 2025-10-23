using UnityEngine;

public class MusicPlayer
{
    BGMTrack[] tracks =
    {
        new BGMTrack("Amen Break", "AmenBreak", AkEventList.instance.play_amen_break, AkEventList.instance.stop_amen_break)
    };
}
