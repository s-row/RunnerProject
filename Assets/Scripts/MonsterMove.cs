using UnityEngine;

// 몬스터 이동 타입
public enum MonsterType
{
    Frog,
    Normal
}

// 몬스터 타입에 따라 일반 이동 또는 개구리 점프 이동을 처리
public class MonsterMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float destroyDistance = 15f;
    public MonsterType monsterType;

    [Header("Frog Monster")]
    public float frogJumpPower = 3f;
    public float frogJumpInterval = 1.2f;

    private Rigidbody2D rb;
    private Transform player;
    private Camera mainCamera;
    private Animator animator;

    private int moveDirection;
    private bool isGrounded = false;
    private float jumpTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Init(Transform targetPlayer)
    {
        player = targetPlayer;
        mainCamera = Camera.main;

        // 스폰 시점의 플레이어 위치를 기준으로 이동 방향 결정
        if (player.position.x > transform.position.x)
        {
            moveDirection = 1;
        }
        else
        {
            moveDirection = -1;
        }
    }

    private void FixedUpdate()
    {
        switch (monsterType)
        {
            case MonsterType.Normal:
                NormalMove();
                break;

            case MonsterType.Frog:
                FrogMove();
                break;
        }
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            return;
        }

        // 카메라 밖으로 멀어진 몬스터는 제거해서 불필요한 오브젝트를 줄임
        float distanceFromCamera = Mathf.Abs(transform.position.x - mainCamera.transform.position.x);

        if (distanceFromCamera > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void NormalMove()
    {
        // 일반 몬스터는 정해진 방향으로 직선 이동
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    private void FrogMove()
    {
        jumpTimer += Time.fixedDeltaTime;

        // 개구리는 이동 방향으로 계속 전진
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

        // 바닥에 있을 때 일정 간격마다 점프
        if (isGrounded == true && jumpTimer >= frogJumpInterval)
        {
            rb.linearVelocity = new Vector2(moveDirection * speed, frogJumpPower);
            jumpTimer = 0f;
            isGrounded = false;

            animator.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿으면 착지 처리
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;

            if (monsterType == MonsterType.Frog)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}