using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordCheckInputField;

    public TMP_Text messageText;

    private Coroutine messageCoroutine;

    public async void OnClickCompleteRegistrationButton()
    {
        string email = emailInputField.text.Trim();
        string password = passwordInputField.text;
        string passwordCheck = passwordCheckInputField.text;

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

        if (string.IsNullOrEmpty(passwordCheck))
        {
            ShowMessage("Please check your password.");
            return;
        }

        if (password != passwordCheck)
        {
            ShowMessage("Passwords do not match.");
            return;
        }

        if (SupabaseManager.Instance == null)
        {
            ShowMessage("Supabase is not ready.");
            Debug.LogError("SupabaseManager.Instance is null");
            return;
        }

        messageText.text = "Signing up...";

        bool result = await SupabaseManager.Instance.SignUp(email, password);

        if (result == false)
        {
            ShowMessage("Sign up failed. This email may already exist.");
            return;
        }

        messageText.text = "Sign up complete.";
        StartCoroutine(MoveToLoginSceneAfterDelay());
    }

    public void OnClickCancelButton()
    {
        SceneManager.LoadScene("LoginScene");
    }

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

    private IEnumerator MoveToLoginSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("LoginScene");
    }
}