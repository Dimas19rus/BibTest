using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class RequestOfRentalBooks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PersonalDataId { get; set; }
        [Required]
        public int BookId { get; set; }
        
        public bool? IsAllowed { get; set;}
        
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [ForeignKey(nameof(PersonalDataId))]
        public PersonalData PersonalData { get; set; }

    }
}
