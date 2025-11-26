using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float durability;
    public WeaponType type;
    public float startWidth;
    public float startHeight;
    public float endWidth;
    public float endHeight;
    public Sprite attackEffect;
    public Sprite weaponSprite;
}
