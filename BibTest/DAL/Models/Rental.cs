using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int PersonalDataId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRental { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRentalBefore { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateReturn { get; set; }
        
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        
        [ForeignKey(nameof(PersonalDataId))]
        public PersonalData PersonalData { get; set; }
    }
}
