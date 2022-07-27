using BibTest.DAL.Models;
using BibTest.Models.General;

namespace BibTest.Models.Books
{
    public class BooksModel
    {
        public BooksModel(){}

        public BooksModel(Notice notice)
        {
            Notice = notice;
        }
        public string SearchTextTitle { get; set; } = "";
        public string SearchTextAuthor { get; set; } = "";
        public IEnumerable<Author>? Authors { get; set; }
        public IEnumerable<Book>? Books { get; set; }

        public Notice Notice { get; set; }
    }
}
