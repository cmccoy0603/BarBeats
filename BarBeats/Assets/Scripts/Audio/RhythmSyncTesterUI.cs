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
        float sync_score = GameManager.MusicPlayer.GetRhythmSyncScore();
        syncImage.color = new Vector4(1 - sync_score, 1f, 1 - sync_score, 1f);
    }
}
