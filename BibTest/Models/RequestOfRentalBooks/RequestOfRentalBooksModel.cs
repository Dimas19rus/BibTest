using BibTest.DAL.Models;
using BibTest.Models.General;

namespace BibTest.Models.RequestOfRentalBooks
{
    public class RequestOfRentalBooksModel
    {
        public RequestOfRentalBooksModel() { }
        public RequestOfRentalBooksModel(Notice notice)
        {
            Notice = notice;
        }
        public IEnumerable<DAL.Models.RequestOfRentalBooks> RequestOfRentalBooks { get; set; }
        public string SearchText { get; set; }
        public Notice Notice { get; set; }
    }
}
