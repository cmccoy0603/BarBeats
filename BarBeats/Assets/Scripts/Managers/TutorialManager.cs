using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject syncTester;
    [SerializeField] private GameObject tutorialUI;

    public void EndTutorial()
    {
        syncTester.SetActive(false);
        GameManager.PlayerManager.PlayerStats.DamageMult = 1.0f;
        GameManager.IsTutorial = false;
        tutorialUI.SetActive(false);
    }
}
