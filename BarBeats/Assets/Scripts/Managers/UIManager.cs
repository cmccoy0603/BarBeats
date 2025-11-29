using Enums;
using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject upgradeText1;
    [SerializeField] private GameObject upgradeText2;
    [SerializeField] private GameObject upgradeText3;

    private float upgradeGoal = 100;
    private int upgrade1;
    private int upgrade2;
    private int upgrade3;
    private int[] UpgradeOptions = {1, 2, 3, 4, 5};

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
            PauseGame();
            pauseMenu.SetActive(true);
        }
        else if (gameState == GameState.PAUSED)
        {
            // Close the menu as well if we press escape
            if (settingsMenu.activeSelf)
            {
                LoadMenu();
            }
            UnpauseGame();
            pauseMenu.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        pauseMenu.SetActive(false);
        Boolean isActive = settingsMenu.activeSelf;
        settingsMenu.SetActive(!isActive);
    }

    public void PauseGame()
    {
        GameManager.GameState = GameState.PAUSED;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        GameManager.GameState = GameState.PLAYING;
        Time.timeScale = 1f;
    }

    public void UpgradePlayer()
    {
        PauseGame();

        // This creates a clone of UpgradeOptions, shuffles it for pure random chance, and then assigns it to vars upgrade1-3.
        int[] shuffled = UpgradeOptions;

        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]); // swap
        }

        upgrade1 = shuffled[0];
        upgrade2 = shuffled[1];
        upgrade3 = shuffled[2];


        upgradeMenu.SetActive(true);
    }

    public void UpgradeClicked(int upgradeChoice)
    {
        switch(upgradeChoice)
        {
            case 1:
                ApplyUpgrades(upgrade1);
                break;
            case 2:
                ApplyUpgrades(upgrade2);
                break;
            case 3:
                ApplyUpgrades(upgrade3);
                break;
            default:
                break;
        }    
        UnpauseGame();
        upgradeMenu.SetActive(false);
    }

    public void ApplyUpgrades(int chosenUpgrade)
    {
        return;
    }
}
