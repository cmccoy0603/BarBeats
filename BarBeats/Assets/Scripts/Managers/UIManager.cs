using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameOver;

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

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
