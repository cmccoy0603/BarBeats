using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;

    public void UpdateScore(float amount)
    {
        float currScore;
        try
        {
            currScore = float.Parse(score.text);
        }
        catch (Exception e)
        {
            // This shouldn't happen...
            currScore = 0f;
        }

        currScore += amount;
        score.text = currScore.ToSafeString();
    }
}
