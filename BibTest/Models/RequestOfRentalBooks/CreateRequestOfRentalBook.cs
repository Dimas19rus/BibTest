using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibTest.Models.RequestOfRentalBooks
{
    public class CreateRequestOfRentalBook
    {
        [Required]
        public int BookId { get; set; }

        [ValidateNever]
        public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();
    }
}
