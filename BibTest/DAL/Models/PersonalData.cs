using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class PersonalData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }

        [InverseProperty(nameof(Models.User.PersonalData))]
        public User User { get; set; }

        [InverseProperty(nameof(Models.Rental.PersonalData))]
        public ICollection<Rental> DistributionOfBooks { get; set; }
    }
}
