using UnityEngine;

// 플레이어의 바닥 체크, 충돌 감지, 몬스터 접촉 처리를 담당
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
        // 발밑 위치에 바닥 Layer가 있는지 검사
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
        // Trigger 오브젝트는 Key, Goal 같은 통과형 오브젝트 처리에 사용
        ObjectItem objectItem = collision.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleObject(playerStatus, objectItem);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 몬스터와 닿아있는 동안 일정 간격으로 데미지를 받도록 코루틴 실행
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

        // 충돌형 오브젝트는 Block 같은 물리 충돌 오브젝트 처리에 사용
        ObjectItem objectItem = collision.gameObject.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleCollisionObject(playerStatus, objectItem);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 몬스터와 떨어지면 반복 데미지를 중지
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

        // Scene 창에서 바닥 체크 범위를 확인하기 위한 표시
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}