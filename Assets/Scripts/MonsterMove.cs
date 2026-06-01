using UnityEngine;

public enum MonsterType
{
    Frog,
    Normal
}

public class MonsterMove : MonoBehaviour
{
    public float speed = 2f;
    public float destroyDistance = 15f;
    public MonsterType monsterType;
    

    [Header("Frog Monster")]
    public float frogJumpPower = 3f;      // 개구리 점프 힘, 플레이어보다 낮게
    public float frogJumpInterval = 1.2f;   // 몇초마다 점프하는지

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

        float distanceFromCamera = Mathf.Abs(transform.position.x - mainCamera.transform.position.x);

        if (distanceFromCamera > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void NormalMove()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }
    private void FrogMove()
    {
        jumpTimer += Time.fixedDeltaTime;

        // 공중이든 바닥이든 x축으로는 계속 이동
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

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
