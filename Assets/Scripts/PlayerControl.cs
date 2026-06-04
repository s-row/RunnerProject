using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Ground Check")]
    public bool isGrounded = false;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("References")]
    public ObjectManager objectManager;
    public PlayerStatus playerStatus;

    private bool isTouchingMonster = false;
    private Coroutine monsterDamageCoroutine;

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        playerStatus.SetGroundedAnimation(isGrounded);
    }

    public void SetRunAnimation(bool isRun)
    {
        playerStatus.SetRunAnimation(isRun);
    }

    public void StartJumpAnimation()
    {
        playerStatus.StartJumpAnimation();
    }

    public bool IsHit()
    {
        return playerStatus.IsHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectItem objectItem = collision.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleObject(playerStatus, objectItem);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            isTouchingMonster = true;

            if (monsterDamageCoroutine == null)
            {
                monsterDamageCoroutine = StartCoroutine(
                    playerStatus.MonsterDamageCoroutine(() => isTouchingMonster)
                );
            }

            return;
        }

        ObjectItem objectItem = collision.gameObject.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleCollisionObject(playerStatus, objectItem);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            isTouchingMonster = false;

            if (monsterDamageCoroutine != null)
            {
                StopCoroutine(monsterDamageCoroutine);
                monsterDamageCoroutine = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}