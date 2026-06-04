using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// 로그인 입력 검사, Supabase 로그인 요청, 씬 이동을 담당
public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text messageText;

    private Coroutine messageCoroutine;

    private void ShowMessage(string message)
    {
        messageText.text = message;

        // 기존 메시지 삭제 코루틴이 있으면 중지해서 새 메시지가 바로 사라지는 것을 방지
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ClearMessageAfterDelay());
    }

    private IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        messageText.text = "";
        messageCoroutine = null;
    }

    public async void OnClickLoginButton()
    {
        string email = emailInputField.text.Trim();
        string password = passwordInputField.text;

        // 입력값 검증
        if (string.IsNullOrEmpty(email))
        {
            ShowMessage("Please enter your email.");
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowMessage("Please enter your password.");
            return;
        }

        messageText.text = "Logging in...";

        // MainScene부터 실행하지 않아 SupabaseManager가 없는 경우 방지
        if (SupabaseManager.Instance == null)
        {
            ShowMessage("Supabase is not ready.");
            Debug.LogError("SupabaseManager.Instance is null");
            return;
        }

        // Supabase 로그인 요청
        bool result = await SupabaseManager.Instance.Login(email, password);

        if (result == true)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            ShowMessage("Login failed.");
        }
    }

    public void OnClickSignUpButton()
    {
        SceneManager.LoadScene("CreateAccountScene");
    }
}