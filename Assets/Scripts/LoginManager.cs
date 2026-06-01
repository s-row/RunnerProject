using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text messageText;

    private Coroutine messageCoroutine;
    private void ShowMessage(string message)
    {
        messageText.text = message;

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