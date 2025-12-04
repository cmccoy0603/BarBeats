using System;
using UnityEngine;
using UnityEngine.UI;

public class RhythmSyncTesterUI : MonoBehaviour
{
    private Image syncImage;

    private void Awake()
    {
        syncImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float sync_score = GameManager.MusicPlayer.GetTempoSyncScore();
        float bar_sync_score = GameManager.MusicPlayer.GetBarSyncScore();
        syncImage.color = new Vector4(1 - sync_score, 1 - sync_score / 2 + bar_sync_score / 2, 1 - bar_sync_score, 1);
    }
}
