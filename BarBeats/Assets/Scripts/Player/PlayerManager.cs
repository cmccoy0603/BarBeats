using Enums;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerAttack playerAttack;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private GameState _gameState = GameState.PLAYING;

    public void AddScreenShake(float magnitude, float duration)
    {
        cameraManager.AddScreenShake(magnitude, duration);
    }

    public void HitScreenShake()
    {
        cameraManager.AddScreenShake(.1f, .25f);
    }

    public void MovePlayer(Vector2 direction)
    {
        playerMovement.FollowThrough(direction);
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
}
