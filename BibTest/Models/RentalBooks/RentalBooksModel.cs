using BibTest.DAL.Models;
using BibTest.Models.General;

namespace BibTest.Models.RentalBooks
{
    public class RentalBooksModel
    {
        public RentalBooksModel() { }
        public RentalBooksModel(Notice notice)
        {
            Notice = notice;
        }
        public IEnumerable<Rental> RentalBooks { get; set; }
        public string SearchText { get; set; }
        public Notice Notice { get; set; }
    }
}
