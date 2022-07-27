using BibTest.DAL;
using BibTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using BibTest.DAL.Models;
using BibTest.Models.Account;
using BibTest.Models.Books;
using BibTest.Models.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BibTest.Controllers
{
    public class BooksController : Controller
    {

        private AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BooksModel model = new BooksModel { Books = await GetBooks(), Authors = await GetAuthors() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BooksModel model)
        {
            model.Books = await GetBooks(model.SearchTextTitle);
            model.Authors = await GetAuthors();
            return View(model);
        }

        public async Task<IActionResult> AuthorsBooks(int authorId)
        {
            var author = GetAuthor(authorId);
            BooksModel model = new BooksModel { Books = await GetBooks(authorId), Authors = await GetAuthors() };
            if (author is not null)
            {
                model.SearchTextAuthor = $"{author.Surname} {author.Name} {author.Patronymic}";
                return View("Index", model);
            }

            model.Notice = new Notice { TypeNotice = TypeNoticeEnum.danger, Message = "Данный автор не был найден" };
            return View("Index", model);
        }

        public async Task<IActionResult> Details(int bookId, Notice notice)
        {
            var book = GetBook(bookId);
            Author? author = null;
            if (book != null)
            {
                author = GetAuthor(book.AuthorId);
            }
            DetailsBookModel model = new DetailsBookModel();
            if (book == null)
            {
                return RedirectToAction("Index",
                    new BooksModel(
                        new Notice
                        {
                            TypeNotice = TypeNoticeEnum.danger,
                            Message = "Данная книга по непонятным причинам не была найдена!!!"
                        }));
            }

            model.BookId = bookId;
            model.Title = book.Title;
            model.Description = book.Description;
            model.PathBookCover = book.PathBookCover;
            model.PathBookData = book.PathBookData;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                model.IsThereBook = await IsThereBook(bookId);
                model.IsRequestBook = await IsRequestBook(bookId);
            }

            if (author != null)
            {
                model.AuthorName = author.Surname + " " + author.Name + " " + author.Patronymic;
                model.AuthorId = author.Id;
            }

            model.Notice = notice;
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownloadBookData(int bookId, string path)
        {
            if (!User.IsInRole("Библиотекарь"))
            {
                if (!await IsThereBook(bookId))
                    return RedirectToAction("Details", new
                    {
                        TypeNotice = TypeNoticeEnum.warning,
                        Message = "Данная книга не выдана вам!",
                        bookId = bookId
                    });
            }

            string filePath = _webHostEnvironment.WebRootPath + path.Replace('/', '\\');
            FileInfo info = new FileInfo(filePath);
            return File(path, "text/plain", info.Name);
        }

        private async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }
        private async Task<IEnumerable<Book>> GetBooks(string searchText)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchText))
                .Include(b => b.Author)
                .ToListAsync();

        }
        private async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
        private async Task<IEnumerable<Book>> GetBooks(int authorId)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.Author)
                .ToListAsync();

        }
        private Book? GetBook(int bookId)
        {
            return _context.Books
                .FirstOrDefault(b => b.Id == bookId);
        }
        private Author? GetAuthor(int authorId)
        {
            return _context.Authors
                .FirstOrDefault(a => a.Id == authorId);
        }
        private async Task<int> GetIdPersonalData()
        {
            IEnumerable<User> users = await _context.Users
                .Where(u => u.Login == User.Identity.Name)
                .Include(u => u.PersonalData)
                .ToListAsync();

            return users.First().PersonalDataId;
        }
        private async Task<bool> IsThereBook(int bookId)
        {
            int idPersonalData = await GetIdPersonalData();
            IEnumerable<int> renInts = await _context.Rentals
                .Where(d => d.PersonalDataId == idPersonalData && d.DateReturn == null)
                .Select(d => d.BookId)
                .ToListAsync();
            return renInts.Contains(bookId);
        }
        private async Task<bool> IsRequestBook(int bookId)
        {
            int idPersonalData = await GetIdPersonalData();
            IEnumerable<int> request = await _context.RequestOfRentalBooks
                .Where(r => r.PersonalDataId == idPersonalData && r.IsAllowed != false)
                .Select(r => r.BookId)
                .ToListAsync();
            return request.Contains(bookId);
        }



    }
}