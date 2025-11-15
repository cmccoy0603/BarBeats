using UnityEngine;
using UnityEngine.Events;

public class OnHit : MonoBehaviour
{
    public UnityEvent<GameObject> OnHitEnemyEvent;
    public UnityEvent<GameObject> OnKillEnemyEvent;
    
    public void OnHitEnemy(GameObject hitCreature)
    {
        OnHitEnemyEvent?.Invoke(hitCreature);
    }

    public void OnKillEnemy(GameObject killedCreature)
    {
        OnKillEnemyEvent?.Invoke(killedCreature);
    }
}
