using System.Data;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private GameObject dropPrefab;
    
    private WeaponHolder holder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holder = GetComponentInChildren<WeaponHolder>();
    }

    // Drop the weapon at the spot the owner of this component is
    public void Drop()
    {
        WeaponData toDrop;
        float durability;
        
        // We want to get the weapon data either through the holders weapon (if there is one) or just create a new one
        if (holder)
        {
            toDrop = holder.EquippedWeaponData();
            durability = holder.CurrentDurability();
        }
        else
        {
            toDrop = data;
            durability = data.durability;
        }
        
        var droppedWeapon = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // Get the drop weapon component
        DroppedWeapon dropInfo = droppedWeapon.GetComponent<DroppedWeapon>();
        if (!dropInfo)
        {
            Debug.LogError("Ensure dropped weapon prefab has DroppedWeapon component attached");
        }
        dropInfo.Drop(toDrop, durability);
    }
    
    public void Drop(WeaponData data, float durability)
    {
        var droppedWeapon = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // Get the drop weapon component
        DroppedWeapon dropInfo = droppedWeapon.GetComponent<DroppedWeapon>();
        if (!dropInfo)
        {
            Debug.LogError("Ensure dropped weapon prefab has DroppedWeapon component attached");
        }
        dropInfo.Drop(data, durability);
    }
}
