using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

// Supabase scores 테이블과 매칭되는 점수 데이터 모델
[Table("scores")]
public class ScoreRecord : BaseModel
{
    // DB에서 자동 생성되는 기본키
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    // Supabase Auth 유저 고유 ID
    [Column("user_id")]
    public string UserId { get; set; }

    // 로그인한 유저 이메일
    [Column("user_email")]
    public string UserEmail { get; set; }

    // 최종 점수
    [Column("score")]
    public int Score { get; set; }

    // 피격 횟수
    [Column("hit_count")]
    public int HitCount { get; set; }
}