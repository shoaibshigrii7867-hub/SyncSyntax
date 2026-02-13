using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncSyntax.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(400, ErrorMessage = "Title cannot exceed 400 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Author is required")]
        [MaxLength(50, ErrorMessage = "Author name cannot exceed 50 characters")]
        public string Author { get; set; }
        [ValidateNever]
        public string FutureImageURL { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }= DateTime.Now;
        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }

        public ICollection<Comments>? Comments { get; set; }    


    }
}
