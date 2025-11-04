using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private float scoreAmount = 10f;

    public void IncreaseScore()
    {
        GameManager.UiManager.UpdateScore(scoreAmount);
    }
}
