using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponDisplay;
    [SerializeField] private Image durabilityRect;

    public void UpdateDisplay(String name)
    {
        weaponDisplay.text = name;
    }

    public void UpdateDurability()
    {
        float ratio = GameManager.PlayerManager.GetWeaponDurabilityRatio();
        Vector3 localScale = durabilityRect.transform.localScale;
        durabilityRect.transform.localScale = new Vector3(ratio, localScale.y, localScale.z);
    }
}
