using System;
using Enums;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerAttack playerAttack;

    [SerializeField] public Health playerHealth;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public PlayerStats PlayerStats;
    
    public UnityEvent OnUpgrade;

    private GameState _gameState = GameState.PLAYING;

    private void Awake()
    {
        if (!GameManager.PlayerManager)
        {
            GameManager.PlayerManager = this;
        }
    }

    public void AddScreenShake(float magnitude, float duration)
    {
        cameraManager.AddScreenShake(magnitude, duration);
    }

    public void HitScreenShake()
    {
        cameraManager.AddScreenShake(.1f, .25f);
    }

    public void HitScreenZoom()
    {
        cameraManager.ZoomIn();
    }

    public void MovePlayer(Vector2 direction)
    {
        playerMovement.FollowThrough(direction);
    }

    public void IncreaseMaxHealth(float amount)
    {
        playerHealth.IncreaseMaxHealth(amount);
    }

    public void ApplyUpgrade()
    {
        OnUpgrade?.Invoke();
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public bool IsGameOver()
    {
        return _gameState == GameState.GAMEOVER;
    }

    public void PlayerDeath()
    {
        Debug.Log("Player died :'(");
        _gameState = GameState.GAMEOVER;
        GameManager.UiManager.ShowGameOver();
        spriteRenderer.enabled = false;
    }

    public float GetWeaponDurabilityRatio()
    {
        return playerAttack.weaponHolder.GetDurabilityRatio();
    }

    public void UpdateHealth()
    {
        GameManager.UiManager.UpdateHealth();
    }
}
