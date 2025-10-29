using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float attackRange = 0.5f;
    public GameObject attackCircle;

    private Camera cam;
    // We'll probably want a weapon for the player to have. That way we can switch it out. For now, nah
    private InputAction _positionAction;

    private Vector2 _widthHeight = Vector2.zero;

    private void Awake()
    {
        cam = Camera.main;
        _positionAction = InputSystem.actions.FindAction("PointerPosition");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _widthHeight = spriteRenderer.size;
        attackCircle.transform.localScale = new Vector3(attackRange, attackRange, 1);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        float rhythmScore = 1.0f;
        //rhythmScore = musicPlayer.getRyhthmSyncScore();
        
        // TODO: Play animation depending on the rhythm score

        Vector3 inputPosition = _positionAction.ReadValue<Vector2>();

        Vector2 aimVector = inputPosition;
        Debug.Log(context.control.shortDisplayName);
        // Our math changes slightly if the player is using a mouse vs a keyboard. We can probably abstract this out in the future
        if (context.control.shortDisplayName == "LMB")
        {
            inputPosition.z = cam.nearClipPlane;
            Vector3 point = cam.ScreenToWorldPoint(inputPosition);

            // Now we get the vector from the player to the point (aim direction)
            aimVector = point - transform.position;
        }

        aimVector.Normalize();
        
        // Angle in rad
        float angle = Mathf.Atan2(aimVector.y, aimVector.x);
        Vector2 startPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _widthHeight;
        attackCircle.transform.localPosition = startPos;

        // Do something with this... I am thinking we'll do something where we can plug in other weapons?
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(startPos, attackRange, enemyLayer);
        
        foreach(Collider2D hitEnemy in hitEnemies)
        {
            Debug.Log("hit an enemy");
        }
    }
}
