using UnityEngine;
using UnityEngine.SceneManagement;

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