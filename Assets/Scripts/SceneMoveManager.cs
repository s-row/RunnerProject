using UnityEngine;
using UnityEngine.SceneManagement;

// UI 버튼에서 호출할 씬 이동 함수들을 모아둔 매니저
public class SceneMoveManager : MonoBehaviour
{
    public void MoveToLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void MoveToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MoveToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MoveToCreateAccountScene()
    {
        SceneManager.LoadScene("CreateAccountScene");
    }
}