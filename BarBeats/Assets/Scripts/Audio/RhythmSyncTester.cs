using System;
using UnityEngine;

public class RhythmSyncTester : MonoBehaviour
{
    private const float OSCILLOSCOPE_X = -1;
    private const float OSCILLOSCOPE_Y = 1;
    private const float OSCILLOSCOPE_WIDTH = 5;
    private const float OSCILLOSCOPE_HEIGHT = 3;
    private const int NUM_OSCILLOSCOPE_POINTS = 100;
    private const float OSCILLOSCOPE_X_STEP = OSCILLOSCOPE_WIDTH / NUM_OSCILLOSCOPE_POINTS;

    private SpriteRenderer sprite_renderer;
    private LineRenderer line_renderer;
    private Vector3[] oscilloscope_points = new Vector3[NUM_OSCILLOSCOPE_POINTS];
    private uint oscilloscope_index = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite_renderer = GetComponentInChildren<SpriteRenderer>();
        line_renderer = GetComponentInChildren<LineRenderer>();
        line_renderer.positionCount = NUM_OSCILLOSCOPE_POINTS;
    }

    // Update is called once per frame
    void Update()
    {
        float sync_score = GameManager.MusicPlayer.GetTempoSyncScore();
        sprite_renderer.color = new Vector4(1 - sync_score, 1f, 1 - sync_score, 1f);
        // oscilloscope_points[oscilloscope_index] = transform.position + new Vector3(
        //     OSCILLOSCOPE_X + OSCILLOSCOPE_X_STEP * oscilloscope_index,
        //     OSCILLOSCOPE_Y + OSCILLOSCOPE_HEIGHT * sync_score
        // );
        // line_renderer.SetPositions(oscilloscope_points);
        // oscilloscope_index++;
        // oscilloscope_index %= NUM_OSCILLOSCOPE_POINTS;
    }
}
