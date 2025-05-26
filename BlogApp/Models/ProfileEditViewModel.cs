using BlogApp.Entity;

namespace BlogApp.Models
{
    public class ProfileEditViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? EMail { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? ResetToken { get; set; } // Şifre sıfırlama için kullanılacak token
        public DateTime? ResetTokenExpire { get; set; } // Token süresi
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
