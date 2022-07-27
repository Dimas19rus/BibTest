using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibTest.Models.RentalBooks
{
    public class DetailsRentalBook
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int PersanalDataId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRental { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRentalBefore { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateReturn { get; set; }


        [ValidateNever]
        public List<SelectListItem> PersonalData { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();

       
    }
}
