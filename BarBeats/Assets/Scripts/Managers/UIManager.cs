using Enums;
using System;
using System.Linq;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private TextMeshProUGUI upgradeText1;
    [SerializeField] private TextMeshProUGUI upgradeText2;
    [SerializeField] private TextMeshProUGUI upgradeText3;

    private float upgradeGoal = 100;
    private int upgrade1;
    private int upgrade2;
    private int upgrade3;
    private int[] UpgradeOptions = { 1, 2, 3, 4, 5 };

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
            upgradeGoal *= 2f;
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
            AkEventList.instance.paused_state_paused.SetValue();
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
            AkEventList.instance.paused_choice_unpaused.SetValue();
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

        ApplyUpgradeText(upgrade1, upgradeText1);
        ApplyUpgradeText(upgrade2, upgradeText2);
        ApplyUpgradeText(upgrade3, upgradeText3);

        upgradeMenu.SetActive(true);
    }

    public void ApplyUpgradeText(int upgrade, TextMeshProUGUI textObject)
    {
        switch (upgrade)
        {
            case 1: // Upgrade player health
                textObject.text = "Upgrade Health: Increases health by +25 (base health of 100)";
                break;
            case 2: // Upgrade player damage
                textObject.text = "Upgrade Damage: Increases damage by +2 (base damage of 10)";
                break;
            case 3: // Upgrade attack spread/width
                textObject.text = "Increase Attack Width: Increases attack width by 0.4 (base of 2)";
                break;
            case 4: // Upgrade lifesteal
                textObject.text = "Upgrade Lifesteal: Increases lifesteal by +1 health gained per attack (base of 0)";
                break;
            case 5: // Upgrade attack range
                textObject.text = "Increase Attack Range: Increases attack range by 0.3 (base of 1.5)";
                break;
            default:
                print("Unknown upgrade");
                break;
        }
        GameManager.PlayerManager.ApplyUpgrade();
    }

    public void UpgradeClicked(int upgradeChoice)
    {
        switch (upgradeChoice)
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
        PlayerStats stats = GameManager.PlayerManager.PlayerStats;
        switch (chosenUpgrade)
        {
            case 1: // Upgrade player health
                GameManager.PlayerManager.IncreaseMaxHealth(25);
                break;
            case 2: // Upgrade player damage
                stats.DamageAdd += 2;
                break;
            case 3: // Upgrade attack spread/width
                stats.AttackSpreadAdd += .2f;
                break;
            case 4: // Upgrade lifesteal
                stats.LifeSteal += 1;
                break;
            case 5: // Upgrade attack range
                stats.AttackRangeAdd += .3f;
                break;
            default:
                print("Unknown upgrade");
                break;
        }
    }
}
