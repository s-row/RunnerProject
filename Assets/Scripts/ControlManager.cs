using UnityEngine;

// 모바일 버튼 입력을 받아 플레이어 이동과 점프를 처리
public class ControlManager : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D playerRb;
    public PlayerControl pct;
    public SpriteRenderer playerSpriteRenderer;

    public float speed = 1.5f;
    public float jump = 2;

    // 0 = 정지, 1 = 오른쪽, -1 = 왼쪽
    private int playerDefaultMove = 0;

    private void FixedUpdate()
    {
        // 버튼 입력값에 따라 X축 이동, Y축은 점프/낙하 속도 유지
        playerRb.linearVelocity = new Vector2(playerDefaultMove * speed, playerRb.linearVelocity.y);

        // 피격 중이 아니고 바닥에 있을 때만 Run/Idle 애니메이션 갱신
        if (pct.IsHit() == false && pct.isGrounded == true)
        {
            pct.SetRunAnimation(playerDefaultMove != 0);
        }
    }

    public void OnPressRightButton()
    {
        playerDefaultMove = 1;
        playerSpriteRenderer.flipX = false;
    }

    public void OnPressLeftButton()
    {
        playerDefaultMove = -1;
        playerSpriteRenderer.flipX = true;
    }

    public void OnReleaseMovebutton()
    {
        playerDefaultMove = 0;
    }

    public bool IsMoving()
    {
        return playerDefaultMove != 0;
    }

    public void OnClickJumpButton()
    {
        Debug.Log("Jump button clicked / isGrounded: " + pct.isGrounded);

        // 바닥에 있을 때만 점프 가능
        if (pct.isGrounded == true)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jump);

            pct.SetRunAnimation(false);
            pct.StartJumpAnimation();
        }
    }
}