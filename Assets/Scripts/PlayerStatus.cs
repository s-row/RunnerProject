using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어의 HP, Key, 피격/회복, 무적 시간, 애니메이션 상태를 관리
public class PlayerStatus : MonoBehaviour
{
    [Header("Status")]
    public int MaxHp = 4;
    public int currentHp;
    public int keyCount = 0;

    [Header("Damage Settings")]
    public float invincibleTime = 2f;
    public float damageInterval = 2f;

    [Header("References")]
    public UIManager uiManager;
    public Animator animator;

    private bool isInvincible = false;
    private bool isHit = false;

    // 착지 순간을 감지하기 위해 이전 프레임의 바닥 상태를 저장
    private bool wasGrounded = false;
    private bool isJumping = false;

    void Start()
    {
        currentHp = MaxHp;

        uiManager.UpdateKeyCount(keyCount);
        uiManager.UpdateHeart(currentHp);
    }

    public void SetGroundedAnimation(bool ground)
    {
        animator.SetBool("isGround", ground);

        // 공중 상태였다가 바닥에 닿은 순간 점프 애니메이션 해제
        if (wasGrounded == false && ground == true)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        wasGrounded = ground;
    }

    public void StartJumpAnimation()
    {
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    public void SetRunAnimation(bool isRun)
    {
        animator.SetBool("isRun", isRun);
    }

    public void AddKey(int amount)
    {
        keyCount += amount;
        uiManager.UpdateKeyCount(keyCount);

        Debug.Log("Current key count: " + keyCount);
    }

    public bool UseKey(int amount)
    {
        if (keyCount < amount)
        {
            Debug.Log("Not enough keys");
            return false;
        }

        keyCount -= amount;
        uiManager.UpdateKeyCount(keyCount);

        Debug.Log("Key used. Current key count: " + keyCount);
        return true;
    }

    public void Heal(int amount)
    {
        if (currentHp >= MaxHp)
        {
            return;
        }

        currentHp += amount;

        if (currentHp > MaxHp)
        {
            currentHp = MaxHp;
        }

        uiManager.UpdateHeart(currentHp);

        Debug.Log("Healed. Current HP: " + currentHp);
    }

    public void Damage(int damage)
    {
        // 무적 중이면 데미지를 무시
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

        // 피격 시 이동/점프 애니메이션을 끄고 Hit 애니메이션 실행
        animator.SetBool("isHit", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isJumping", false);
        animator.SetTrigger("Hit");

        uiManager.UpdateHeart(currentHp);

        Debug.Log("Damaged. Current HP: " + currentHp);

        if (currentHp <= 0)
        {
            GameResultData.CalculateScore();
            SceneManager.LoadScene("EndScene");
            return;
        }

        StartCoroutine(InvincibleCoroutine());
        StartCoroutine(HitAnimationCoroutine());
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    private IEnumerator HitAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.4f);

        isHit = false;
        animator.SetBool("isHit", false);
    }

    // 몬스터와 계속 닿아있는 동안 일정 간격으로 데미지 적용
    public IEnumerator MonsterDamageCoroutine(System.Func<bool> isTouchingMonster)
    {
        while (isTouchingMonster() == true)
        {
            Damage(1);

            yield return new WaitForSeconds(damageInterval);
        }
    }

    public bool IsHit()
    {
        return isHit;
    }
}