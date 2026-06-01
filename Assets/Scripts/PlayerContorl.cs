using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerContorl : MonoBehaviour
{
    public bool isGrounded = false;
    public int MaxHp = 4;
    public int currentHp;
    public int keyCount = 0;
    public float invincibleTime = 2f;
    public float damageInterval = 2f;

    public Rigidbody2D playerRb;
    public Animator animator;
    public ObjectManager objectManager;
    public UIManager uIManager;


    private bool isInvincible = false;
    private bool isHit = false;
    private bool isTouchingMonster = false;
    private Coroutine monsterDamageCoroutine;

    public Transform groundCheck;
    public float groundCheckRadius = 0.15f; //바닥 판정 범위 크기, 크기가 클수록 판정 범위가 커짐
    public LayerMask groundLayer;

    void Start()
    {
        currentHp = MaxHp;
        uIManager.UpdateKeyCount(keyCount);
        uIManager.UpdateHeart(currentHp);
    }
    void Update()
    {
        CheckGround();
    }
    // 땅에 붙어있나 체크
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetBool("isGround", isGrounded);

        if (isGrounded == true && playerRb.linearVelocity.y <= 0.1f)
        {
            animator.SetBool("isJumping", false);
        }
    }
    // 키 획득 처리
    public void AddKey(int amount)
    {
        keyCount += amount;

        uIManager.UpdateKeyCount(keyCount);

        Debug.Log("현재 열쇠 개수: " + keyCount);
    }

    // 키 사용 처리
    public bool UseKey(int amount)
    {
        if (keyCount < amount)
        {
            Debug.Log("열쇠 부족");
            return false;
        }

        keyCount -= amount;
        uIManager.UpdateKeyCount(keyCount);

        Debug.Log("열쇠 사용 / 현재 열쇠 개수: " + keyCount);
        return true;
    }

    // HP처리
    public void Heal(int amount)
    {
        if (currentHp >= MaxHp)
        {
            Debug.Log("이미 체력이 최대입니다.");
            return;
        }

        currentHp += amount;

        if (currentHp > MaxHp)
        {
            currentHp = MaxHp;
        }

        uIManager.UpdateHeart(currentHp);

        Debug.Log("회복 / 현재 HP: " + currentHp);
    }
 
    // 피격처리
    public void Damage(int damage)
    {

        if (isInvincible == true)
        {
            return;
        }

        GameResultData.AddHit();

        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }

        isHit = true;
        animator.SetBool("isHit", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isJumping", false);
        animator.SetTrigger("Hit");

        uIManager.UpdateHeart(currentHp);

        if (currentHp <= 0)
        {
            GameResultData.CalculateScore();
            SceneManager.LoadScene("EndScene");
            return;
        }

        StartCoroutine(InvincibleCoroutine());
        StartCoroutine(HitAnimationCoroutine());
    }
    // 무적시간 코루틴
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }
    // hit 상태 해제 코루틴
    private IEnumerator HitAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.4f);

        isHit = false;
        animator.SetBool("isHit", false);
    }
    // 피격중 애니메이션 비활성화
    public bool IsHit()
    {
        return isHit;
    }
    private IEnumerator MonsterDamageCoroutine()
    {
        while (isTouchingMonster == true)
        {
            Damage(1);

            yield return new WaitForSeconds(damageInterval);
        }

        monsterDamageCoroutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            isTouchingMonster = true;

            if (monsterDamageCoroutine == null)
            {
                monsterDamageCoroutine = StartCoroutine(MonsterDamageCoroutine());
            }

            return;
        }

        ObjectItem objectItem = collision.gameObject.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleCollisionObject(this, objectItem);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectItem objectItem = collision.GetComponent<ObjectItem>();

        if (objectItem == null)
        {
            return;
        }

        objectManager.HandleObject(this, objectItem);
    }

    // 애니메이션
    public void SetRunAnimation(bool isRun)
    {
        animator.SetBool("isRun", isRun);
    }

    public void StartJumpAnimation()
    {
        animator.SetBool("isJumping", true);
    }
}
