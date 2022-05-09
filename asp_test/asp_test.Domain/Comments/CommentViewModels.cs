namespace asp_test.Domain.Comments
{
    public class CommentViewModel : IEntity
    {
        // commentsテーブルの型定義
        public int Id { get; set; }
        public int Movieid { get; set; }
        public int Userid { get; set; }
        public string Comment1 { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // usersテーブルの型定義
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Gender { get; set; }
    }

    public class RegisterCommentViewModel : IEntity
    {
        // Edit用
        public int Id { get; set; }
        // 以下はCreateとEdit共通
        public int Movieid { get; set; }
        public int Userid { get; set; }
        public string Comment1 { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
