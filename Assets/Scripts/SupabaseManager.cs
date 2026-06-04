using System.Threading.Tasks;
using Supabase;
using UnityEngine;

// Supabase 연결, Auth, 점수 저장/랭킹 조회를 담당
public class SupabaseManager : MonoBehaviour
{
    public static SupabaseManager Instance;

    [Header("Supabase Settings")]
    [SerializeField] private string supabaseUrl = "https://cyhjrsojzrwynrwdscat.supabase.co";
    [SerializeField] private string supabaseKey = "sb_publishable_9qpvAn2AslBcilDoHTD92A_QvHT4UKk";

    public Client Client { get; private set; }

    public string CurrentUserEmail { get; private set; }
    public string CurrentUserId { get; private set; }

    private async void Awake()
    {
        // 씬 이동 시 중복 생성 방지
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await InitSupabase();
    }

    private async Task InitSupabase()
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        Client = new Client(supabaseUrl, supabaseKey, options);
        await Client.InitializeAsync();

        Debug.Log("Supabase connected.");
    }

    public async Task<bool> SignUp(string email, string password)
    {
        try
        {
            await Client.Auth.SignUp(email, password);

            Debug.Log("Sign up request completed.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Sign up failed: " + e.Message);
            return false;
        }
    }

    public async Task<bool> Login(string email, string password)
    {
        try
        {
            var session = await Client.Auth.SignIn(email, password);

            if (session == null || session.User == null)
            {
                Debug.LogError("Login failed: Session is null.");
                return false;
            }

            // 점수 저장에 사용할 로그인 유저 정보 저장
            CurrentUserEmail = session.User.Email;
            CurrentUserId = session.User.Id;

            Debug.Log("Login success: " + CurrentUserEmail);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
            return false;
        }
    }

    public void Logout()
    {
        Client.Auth.SignOut();

        CurrentUserEmail = null;
        CurrentUserId = null;
    }

    public async Task<bool> SaveScore(int score, int hitCount)
    {
        try
        {
            if (Client == null)
            {
                Debug.LogError("Supabase client is not ready.");
                return false;
            }

            if (string.IsNullOrEmpty(CurrentUserId) || string.IsNullOrEmpty(CurrentUserEmail))
            {
                Debug.LogError("User is not logged in.");
                return false;
            }

            // scores 테이블에 저장할 데이터 생성
            ScoreRecord scoreRecord = new ScoreRecord
            {
                UserId = CurrentUserId,
                UserEmail = CurrentUserEmail,
                Score = score,
                HitCount = hitCount
            };

            await Client.From<ScoreRecord>().Insert(scoreRecord);

            Debug.Log("Score saved: " + score);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save score failed: " + e.Message);
            return false;
        }
    }

    public async Task<int> GetMyRank(int myScore)
    {
        try
        {
            var response = await Client.From<ScoreRecord>().Get();
            var scores = response.Models;

            int higherScoreCount = 0;

            // 내 점수보다 높은 기록 개수로 랭킹 계산
            foreach (ScoreRecord record in scores)
            {
                if (record.Score > myScore)
                {
                    higherScoreCount++;
                }
            }

            int rank = higherScoreCount + 1;

            Debug.Log("My rank: " + rank);
            return rank;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Load rank failed: " + e.Message);
            return -1;
        }
    }
}