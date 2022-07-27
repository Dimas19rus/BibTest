using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string PathBookCover { get; set; }
        public string PathBookData { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        [InverseProperty(nameof(Models.Author.Books))]
        public Author Author { get; set; }
        
    }
}
