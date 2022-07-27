using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        [InverseProperty(nameof(Book.Author))]
        public virtual ICollection<Book> Books { get; set; }

    }
}
