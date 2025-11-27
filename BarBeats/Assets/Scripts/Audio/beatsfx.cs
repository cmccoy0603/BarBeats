using JetBrains.Annotations;
using UnityEngine;

public class beatsfx : MonoBehaviour
{
    public AK.Wwise.Bank MainBank;
    public AK.Wwise.Event play_perfect_hit;
    public AK.Wwise.Event play_late_hit;

    private float lastRhythmScore = -1f; 
    private float timeBetweenBeats = 0.5f; 
    private float lastBeatTime = -1f; 
    private bool isSoundTriggeredThisFrame = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainBank.Load();
        
    }


    // Update is called once per frame
    void Update()
    {
       
            float rhythumScore = GameManager.MusicPlayer.GetRhythmSyncScore();
            Debug.Log(rhythumScore);

            float currentTime = Time.time;

            if (!isSoundTriggeredThisFrame)
            {

                if (rhythumScore >= .5f)
                {
                    play_perfect_hit.Post(gameObject);
                    isSoundTriggeredThisFrame = true;
                    lastBeatTime = currentTime;
                }
            else if (rhythumScore >= .01f && rhythumScore < .5f)
            {

                play_late_hit.Post(gameObject);
                isSoundTriggeredThisFrame = true;
                lastBeatTime = currentTime;

            }
        }
        if (currentTime - lastBeatTime >= timeBetweenBeats)
        {
            isSoundTriggeredThisFrame = false; 
        }
    }

   

    private void OnDestroy()
    {
        MainBank.Unload();
    }
}
