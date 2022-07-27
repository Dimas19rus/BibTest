using BibTest.DAL.Models;
using BibTest.Models.General;

namespace BibTest.Models.Books
{
    public class DetailsBookModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string PathBookCover { get; set; }
        public string PathBookData { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public bool IsThereBook { get; set; }
        public bool IsRequestBook { get; set; }
        public Notice Notice { get; set; }   
    }
}
