using System;
using Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private float upgradeGoal = 100;

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

        if (currScore >= upgradeGoal)
        {
            UpgradePlayer();
            upgradeGoal *= 1.5f;
        }

    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    // Flips the menu between pause and unpause
    public void Pause()
    {
        var gameState = GameManager.GameState;
        if (gameState == GameState.PLAYING)
        {
            GameManager.GameState = GameState.PAUSED;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else if (gameState == GameState.PAUSED)
        {
            // Close the menu as well if we press escape
            if (settingsMenu.activeSelf)
            {
                LoadMenu();
            }
            GameManager.GameState = GameState.PLAYING;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        pauseMenu.SetActive(false);
        Boolean isActive = settingsMenu.activeSelf;
        settingsMenu.SetActive(!isActive);
    }

    public void UpgradePlayer()
    {
        print("Upgrade Player");
    }
}
