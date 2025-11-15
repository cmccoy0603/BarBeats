using JetBrains.Annotations;
using UnityEngine;

public class beatsfx : MonoBehaviour
{
    public AK.Wwise.Bank MainBank;
    public AK.Wwise.Event play_perfect_hit;
    public AK.Wwise.Event play_late_hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainBank.Load();
       
    }


    // Update is called once per frame
    void Update()
    {
        float rhythumScore = GameManager.MusicPlayer.GetRhythmSyncScore();

        if (rhythumScore > 0)
        {
            play_perfect_hit.Post(gameObject);
        }
        if (rhythumScore <= 0) {
        
           play_late_hit.Post(gameObject);
        }
    }

    private void OnDestroy()
    {
        MainBank.Unload();
    }
}
