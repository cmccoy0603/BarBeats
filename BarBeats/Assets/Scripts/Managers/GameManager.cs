using System.Collections;
using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The different manager objects we assign to the object
    public MusicPlayer musicPlayer;
    public UIManager uiManager;
    public PlayerManager playerManager;
    public TutorialManager tutorialManager;

    // The static managers we can access anywhere
    public static MusicPlayer MusicPlayer;
    public static UIManager UiManager;
    public static PlayerManager PlayerManager;
    public static GameState GameState;
    public static TutorialManager TutorialManager;
    public static bool IsTutorial = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicPlayer = musicPlayer;
        UiManager = uiManager;
        PlayerManager = playerManager;
        TutorialManager = tutorialManager;
        GameState = GameState.PLAYING;
        IsTutorial = true;
        StartCoroutine(EnsureUnpausedWwiseState());
    }

    private IEnumerator EnsureUnpausedWwiseState()
    {
        while (AkEventList.NullCheck())
        {
            yield return null;
        }
        AkEventList.instance.paused_choice_unpaused.SetValue();
    }
}
