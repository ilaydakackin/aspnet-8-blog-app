using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CreatePostViewModel
    {
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Baslik")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "İcerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Decription { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? url { get; set; }
        public DateTime PublishedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
