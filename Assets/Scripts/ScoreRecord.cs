using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("scores")]
public class ScoreRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("user_email")]
    public string UserEmail { get; set; }

    [Column("score")]
    public int Score { get; set; }

    [Column("hit_count")]
    public int HitCount { get; set; }
}