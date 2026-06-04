using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 결과 화면에서 점수 표시, 점수 저장, 랭킹 조회, 씬 이동을 담당
public class EndSceneManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text rankText;

    private async void Start()
    {
        scoreText.text = "Score : " + GameResultData.finalScore;
        rankText.text = "Rank : Loading...";

        // MainScene부터 실행하지 않으면 SupabaseManager가 없을 수 있음
        if (SupabaseManager.Instance == null)
        {
            Debug.LogError("SupabaseManager does not exist. Start from MainScene.");
            rankText.text = "Rank : Error";
            return;
        }

        // 최종 점수와 피격 횟수를 Supabase scores 테이블에 저장
        bool saveResult = await SupabaseManager.Instance.SaveScore(
            GameResultData.finalScore,
            GameResultData.hitCount
        );

        if (saveResult == true)
        {
            Debug.Log("Score saved in EndScene.");

            // 현재 점수를 기준으로 내 랭킹 계산
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