using UnityEngine;

enum PlayerState
{
    Move,
    Stay,
    Jump
}

public class ControlManager : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D playerRb;
    public PlayerContorl pct;
    public SpriteRenderer playerSpriterRenderer;

    public float speed = 1.5f;
    public float jump = 2;
    
    private int playerDefaultMove = 0; // 0이면 제자리 1이면 오른쪽 -1이면 왼쪽

    private void FixedUpdate()
    {
        playerRb.linearVelocity = new Vector2( playerDefaultMove * speed, playerRb.linearVelocity.y);
        if (pct.IsHit() == false && pct.isGrounded == true)
        {
            pct.SetRunAnimation(playerDefaultMove != 0);
        }
    }
    public void OnPressRightButton()
    {

        playerDefaultMove = 1;
        playerSpriterRenderer.flipX = false;

    }
    
    public void OnPressLeftButton()
    {
        
        playerDefaultMove = -1;
        playerSpriterRenderer.flipX = true;

    }

    public void OnReleseMovebutton()
    {
        playerDefaultMove = 0;

    }
    public bool IsMoving()
    {
        return playerDefaultMove != 0;
    }
    public void OnClickJumpButton()
    {
        Debug.Log("점프 버튼 눌림 / isGrounded: " + pct.isGrounded);
        if (pct.isGrounded == true)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jump);
            pct.SetRunAnimation(false);
            pct.StartJumpAnimation();
        }
    }
}
