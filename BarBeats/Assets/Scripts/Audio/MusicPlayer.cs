public class MusicPlayer
{
    private static BGMTrack current_track = null;

    public static void SwitchToTrack(string internal_name)
    {
        current_track?.Stop();
        current_track = BGMTrack.TryGet(internal_name);
        current_track?.Play();
    }
}
