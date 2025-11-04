using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaseRigidbody2D : MonoBehaviour
{
    public Transform target;
    public float speed = 4f;
    public float stopDistance = 0.4f;

    Rigidbody2D rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null) 
            target = playerObj.transform;
        else
            Debug.LogWarning("No GameObject with tag 'Player' found in scene.");
        
        // Modify speed so its not uniform
        speed += Random.Range(-1f, 1f);
    }

     void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (target == null)
        {
            print("test");
            return;
        }

        Vector2 direction = (target.position - transform.position);
        float dist = direction.magnitude;
        if (dist > stopDistance)
        {
            direction.Normalize();
            Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    public void SetTarget(Transform t) => target = t;
}
