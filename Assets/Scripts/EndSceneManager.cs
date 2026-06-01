using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text rankText;

    private async void Start()
    {
        scoreText.text = "Score : " + GameResultData.finalScore;
        rankText.text = "Rank : Loading...";

        if (SupabaseManager.Instance == null)
        {
            Debug.LogError("SupabaseManager does not exist. Start from MainScene.");
            rankText.text = "Rank : Error";
            return;
        }

        bool saveResult = await SupabaseManager.Instance.SaveScore(
            GameResultData.finalScore,
            GameResultData.hitCount
        );

        if (saveResult == true)
        {
            Debug.Log("Score saved in EndScene.");

            int rank = await SupabaseManager.Instance.GetMyRank(GameResultData.finalScore);

            if (rank > 0)
            {
                rankText.text = "Rank : " + rank;
            }
            else
            {
                rankText.text = "Rank : Error";
            }
        }
        else
        {
            Debug.LogError("Failed to save score in EndScene.");
            rankText.text = "Rank : Save Failed";
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}