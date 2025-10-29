using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The different manager objects we assign to the object
    public MusicPlayer musicPlayer;
    
    // The static managers we can access anywhere
    public static MusicPlayer MusicPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicPlayer = musicPlayer;
    }
}
