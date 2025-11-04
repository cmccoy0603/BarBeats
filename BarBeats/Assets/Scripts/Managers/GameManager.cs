using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The different manager objects we assign to the object
    public MusicPlayer musicPlayer;
    public UIManager uiManager;
    
    // The static managers we can access anywhere
    public static MusicPlayer MusicPlayer;
    public static UIManager UiManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicPlayer = musicPlayer;
        UiManager = uiManager;
    }
}
