using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStatus : MonoBehaviour
{
    public int MaxHp = 4;
    public int currentHp;
    public int keyCount = 0;
    public float invincibleTime = 2f;
    public float damageInterval = 2f;

    public UIManager uiManager;
    public Animator animator;

    private bool isInvincible = false;
    private bool isHit = false;
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
