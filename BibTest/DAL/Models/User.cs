using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibTest.DAL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(Models.Role.Users))]
        public Role Role { get; set; }

        [Required]
        public int PersonalDataId { get; set; }

        [ForeignKey(nameof(PersonalDataId))]
        [InverseProperty(nameof(Models.PersonalData.User))]
        public PersonalData PersonalData { get; set; }

    }
}
