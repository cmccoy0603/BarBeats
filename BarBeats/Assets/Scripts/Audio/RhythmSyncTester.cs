using Unity.Mathematics;
using UnityEngine;

public class RhythmSyncTester : MonoBehaviour
{
    private MusicPlayer music_player;
    private SpriteRenderer sprite_renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music_player = GetComponentInChildren<MusicPlayer>();
        sprite_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float sync_score = music_player.GetRhythmSyncScore();
        sprite_renderer.color = new Vector4(1 - sync_score, 1f, 1 - sync_score, 1f);
    }
}
