using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Stats")]
    public string weaponName;
    public float damage;
    public float fireRate;
    public float durability;
    public WeaponType type;
    [Header("Throw Stats")]
    public float throwSpeed;
    public bool oneShot = true;
    public bool canThrow = true;
    public float thrownDurabilityDec;
    public float swingDurabilityDec;
    [Header("Hitbox")]
    public float startWidth;
    public float startHeight;
    public float endWidth;
    public float endHeight;
    [Header("Visual Effects")]
    public GameObject attackEffect;
    public Sprite weaponSprite;
    public GameObject thrownPrefab;
}
