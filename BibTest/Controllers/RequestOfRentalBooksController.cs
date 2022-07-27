using BibTest.DAL;
using BibTest.DAL.Models;
using BibTest.Models.General;
using BibTest.Models.RentalBooks;
using BibTest.Models.RequestOfRentalBooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BibTest.Controllers
{
    [Authorize]
    public class RequestOfRentalBooksController : Controller
    {
        private AppDbContext _context;

        public RequestOfRentalBooksController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Notice notice, string? searchText)
        {
            RequestOfRentalBooksModel model = new RequestOfRentalBooksModel(notice);
            if (!string.IsNullOrEmpty(searchText))
            {
                model.RequestOfRentalBooks = await GetRequestOfRental(searchText);
                model.SearchText = searchText;
            }
            else
                model.RequestOfRentalBooks = await GetRequestOfRental();
            return View(model);
        }

        [Authorize(Roles = "Читатель")]
        public async Task<IActionResult> Create()
        {
            CreateRequestOfRentalBook model = new CreateRequestOfRentalBook();
            foreach (var item in await GetNotTakenBooks())
            {
                model.Books.Add(new SelectListItem
                    { Value = item.Id.ToString(), Text = item.Title });
            }
            return View(model);
        }

        [Authorize(Roles = "Читатель")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestOfRentalBook model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RequestOfRentalBooks request = new RequestOfRentalBooks
                    {
                        BookId = model.BookId,
                        DateOfCreation = DateTime.Now,
                        PersonalDataId = await GetIdPersonalData()
                    };
                    await _context.RequestOfRentalBooks.AddAsync(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index",
                         new Notice { TypeNotice = TypeNoticeEnum.success, Message = "Запрос успешно отправлен!" });
                }
                foreach (var item in await GetNotTakenBooks())
                {
                    model.Books.Add(new SelectListItem
                        { Value = item.Id.ToString(), Text = item.Title });
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index",
                    new
                    {
                        notice = new Notice { TypeNotice = TypeNoticeEnum.danger, Message = "Произошла ошибка при отправке запроса!" }
                    });
            }
        }

        [Authorize(Roles = "Читатель")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReader(int id,string? searchText)
        {
            try
            {
                var request = await _context.RequestOfRentalBooks.FirstAsync(r => r.Id == id);
                if (request == null) return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.danger,
                        Message = "Запрос не был найден!",
                        searchText = searchText
                    });
                if (request.IsAllowed is not null)
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.warning,
                            Message = "Данный запрос вы не можете удалить так как он является закрытым!",
                            searchText = searchText
                        });
                _context.RequestOfRentalBooks.Remove(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", 
                    new
                    {
                        TypeNotice = TypeNoticeEnum.success,
                        Message = "Запрос успешно удален!",
                        searchText = searchText
                    });
            }
            catch
            {
                return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.danger,
                        Message = "Произошла ошибка!!!",
                        searchText = searchText
                    });
            }
        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLibrarian(int id, string? searchText)
        {
            try
            {
                var request = await _context.RequestOfRentalBooks.FirstAsync(r => r.Id == id);
                if (request == null) return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.danger,
                        Message = "Запрос не был найден!",
                        searchText = searchText
                    });
                if (request.IsAllowed is null)
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.warning,
                            Message = "Данный запрос вы не можете удалить так как он является еще не закрытым!",
                            searchText = searchText
                        });
                _context.RequestOfRentalBooks.Remove(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.success,
                        Message = "Запрос успешно удален!",
                        searchText = searchText
                    });
            }
            catch
            {
                return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.danger,
                        Message = "Произошла ошибка!!!",
                        searchText = searchText
                    });
            }
        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpPost]
        public async Task<IActionResult> Allowed(int id, bool isAllowed, string searchText)
        {
            try
            {
                var request = await _context.RequestOfRentalBooks.FirstAsync(r => r.Id == id);
                if (request == null)
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.danger,
                            Message = "Запрос не был найден!!!",
                            searchText = searchText
                        });
                if (request.IsAllowed == true)
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.warning,
                            Message = "Запрос уже одобрен",
                            searchText = searchText
                        });
                if (request.IsAllowed == false)
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.warning,
                            Message = "Запрос уже был не одобрен",
                            searchText = searchText
                        });
                request.IsAllowed = isAllowed;
                if (isAllowed)
                {
                    Rental rental = new Rental
                    {
                        BookId = request.BookId,
                        PersonalDataId = request.PersonalDataId,
                        DateRental = DateTime.Now,
                        DateRentalBefore = DateTime.Now + new TimeSpan(14, 0, 0, 0)
                    };
                    await _context.Rentals.AddAsync(rental);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.success,
                            Message = "Запрос успешно обновлен. Аренда книги добавлена",
                            searchText = searchText
                        });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.success,
                        Message = "Запрос успешно обновлен",
                        searchText = searchText
                    });
            }
            catch
            {
                return RedirectToAction("Index",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.danger,
                        Message = "Произошла ошибка!!!",
                        searchText = searchText
                    });
            }
        }

        [Authorize(Roles = "Читатель")]
        public async Task<IActionResult> CreateOfDetailBook(int bookId)
        {
            try
            {
                if (!await IsRequestBook(bookId))
                {
                    RequestOfRentalBooks request = new RequestOfRentalBooks
                    {
                        BookId = bookId,
                        DateOfCreation = DateTime.Now,
                        PersonalDataId = await GetIdPersonalData()
                    };
                    await _context.RequestOfRentalBooks.AddAsync(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Books",
                        new
                        {
                            TypeNotice = TypeNoticeEnum.success,
                            Message = "Запрос успешно отправлен!",
                            bookId = bookId
                        });
                }

                return RedirectToAction("Details", "Books",
                    new
                    {
                        TypeNotice = TypeNoticeEnum.warning,
                        Message = "Запрос на данную книгу уже существует!",
                        bookId = bookId
                    });
            }
            catch
            {
                return RedirectToAction("Details", "Books",
                    new
                    {
                        notice = new Notice { TypeNotice = TypeNoticeEnum.danger, Message = "Произошла ошибка при отправке запроса!" }
                    });
            }
        }

        private async Task<IEnumerable<RequestOfRentalBooks>> GetRequestOfRental()
        {
            if (User.IsInRole("Читатель"))
            {
                int idPersonalData = await GetIdPersonalData();
                return await _context.RequestOfRentalBooks
                    .Where(d => d.PersonalDataId == idPersonalData)
                    .Include(d => d.Book)
                    .Include(d => d.PersonalData)
                    .ToListAsync();
            }

            if (User.IsInRole("Библиотекарь"))
            {
                return await _context.RequestOfRentalBooks
                    .Include(d => d.Book)
                    .Include(d => d.PersonalData)
                    .ToListAsync();
            }

            return new List<RequestOfRentalBooks>();
        }
        private async Task<IEnumerable<RequestOfRentalBooks>> GetRequestOfRental(string searchText)
        {
            if (!(User.Identity != null && User.Identity.IsAuthenticated))
                return new List<RequestOfRentalBooks>();
            if (User.IsInRole("Читатель"))
                return await GetRequestOfRentalSearchTitle(searchText);

            if (User.IsInRole("Библиотекарь"))
                return await GetRequestOfRentalSearchPersonalData(searchText);

            return new List<RequestOfRentalBooks>();
        }

        private async Task<IEnumerable<RequestOfRentalBooks>> GetRequestOfRentalSearchPersonalData(string searchText)
        {
            var personalData = await _context.PersonalData
                .Where(p => (p.Surname + " " + p.Name).Contains(searchText))
                .Select(d => d.Id)
                .ToListAsync();

            return await _context.RequestOfRentalBooks
                .Where(d => personalData.Contains(d.PersonalDataId))
                .Include(d => d.Book)
                .Include(d => d.PersonalData)
                .ToListAsync();
        }
        private async Task<IEnumerable<RequestOfRentalBooks>> GetRequestOfRentalSearchTitle(string searchText)
        {
            int idPersonalData = await GetIdPersonalData();
            return await _context.RequestOfRentalBooks
                .Where(d => d.PersonalDataId == idPersonalData)
                .Include(d => d.Book)
                .Where(d => d.Book.Title.Contains(searchText))
                .Include(d => d.PersonalData)
                .ToListAsync();
        }

        private async Task<IEnumerable<Book>> GetNotTakenBooks()
        {
            int idPersonalData = await GetIdPersonalData();
            var rentalBooks = await _context.Rentals
                .Where(r => r.PersonalDataId == idPersonalData && r.DateReturn == null)
                .Include(r=>r.Book)
                .Select(r=>r.Book)
                .ToListAsync();
            var requestBooks = await _context.RequestOfRentalBooks
                .Where(r => r.PersonalDataId == idPersonalData && r.IsAllowed==null)
                .Include(r => r.Book)
                .Select(r => r.Book)
                .ToListAsync();

            return await _context.Books.Where(b=>!rentalBooks.Contains(b) && !requestBooks.Contains(b)).ToListAsync();
        }

        public async Task<RequestOfRentalBooks> GetRequestOfRental(int id)
        {
            return await _context.RequestOfRentalBooks
                .Where(d => d.Id == id)
                .Include(d => d.Book)
                .Include(d => d.PersonalData)
                .FirstAsync();
        }

        private async Task<int> GetIdPersonalData()
        {
            if (!(User.Identity != null && User.Identity.IsAuthenticated))
                return 0;

            IEnumerable<User> users = await _context.Users
                .Where(u => u.Login == User.Identity.Name)
                .Include(u => u.PersonalData)
                .ToListAsync();

            return users.First().PersonalDataId;
        }

        private async Task<bool> IsRequestBook(int bookId)
        {
            int idPersonalData = await GetIdPersonalData();
            IEnumerable<int> request = await _context.RequestOfRentalBooks
                .Where(r => r.PersonalDataId == idPersonalData && r.IsAllowed !=false)
                .Select(r => r.BookId)
                .ToListAsync();
            return request.Contains(bookId);
        }
    }
}
