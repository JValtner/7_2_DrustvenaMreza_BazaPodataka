using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _6_1_drustvena_mreza.DOMEN
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime PostDate { get; set; }

        public Korisnik User { get; set; }

        public Post(int id, int userID, string content, DateTime postDate)
        {
            this.Id = id;
            this.UserId = userID;
            this.Content = content;
            this.PostDate= postDate;
        }
    }
}
