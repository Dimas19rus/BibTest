using BibTest.DAL;
using BibTest.DAL.Models;
using BibTest.Models.General;
using BibTest.Models.RentalBooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibTest.Controllers
{
    [Authorize]
    public class RentalBooksController : Controller
    {
        private readonly AppDbContext _context;

        public RentalBooksController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Notice notice, string searchText)
        {
            RentalBooksModel model = new RentalBooksModel(notice);
            if (searchText != null && searchText != "")
            {
                model.RentalBooks = await GetRentalBooks(searchText);
                model.SearchText= searchText;
            }
            else
                model.RentalBooks = await GetRentalBooks();
            return View(model);
        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            DetailsRentalBook model = new DetailsRentalBook();

            model.DateRental = DateTime.Now;
            model.DateRentalBefore = DateTime.Now + new TimeSpan(14, 0, 0, 0);

            foreach (var item in await GetPersonalData())
            {
                model.PersonalData.Add(new SelectListItem
                { Value = item.Id.ToString(), Text = item.Surname + " " + item.Name });
            }

            foreach (var item in await GetBooks())
            {
                model.Books.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });
            }

            return View(model);

        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsRentalBook model)
        {
            try
            {
                if (model.DateRental > model.DateRentalBefore)
                    ModelState.AddModelError("", "Дата выдачи должна < или = даты запланированного возврата");

                if (ModelState.IsValid)
                {
                    Rental item = new Rental();
                    item.PersonalDataId = model.PersanalDataId;
                    item.BookId = model.BookId;
                    item.DateRentalBefore = model.DateRental;
                    item.DateRental = model.DateRentalBefore;
                    await _context.Rentals.AddAsync(item);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index",
                        new Notice { TypeNotice = TypeNoticeEnum.success, Message = "Запись была успешна добавлена" });
                }

                ModelState.AddModelError("", "Не все поля правильно заполнены!!!");

                foreach (var item in await GetPersonalData())
                {
                    model.PersonalData.Add(new SelectListItem
                    { Value = item.Id.ToString(), Text = item.Surname + " " + item.Name });
                }

                foreach (var item in await GetBooks())
                {
                    model.Books.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });
                }

                return View("Create", model);
            }
            catch
            {
                //_notice.GetMessage(TypeNoticeEnum.danger, "Произошла ошибка. Попробуйте заново создать запись");
                return View("Index");
            }
        }

        [Authorize(Roles = "Библиотекарь")]
        public async Task<IActionResult> Edit(int id)
        {
            DetailsRentalBook model = new DetailsRentalBook();
            var book = await GetRentalBook(id);

            if (book != null)
            {
                model.Id = id;
                model.DateRental = book.DateRental;
                model.DateRentalBefore = book.DateRentalBefore;
                model.DateReturn = book.DateReturn;
                model.BookId = book.BookId;

                foreach (var item in await GetPersonalData())
                {
                    model.PersonalData.Add(new SelectListItem
                    { Value = item.Id.ToString(), Text = item.Surname + " " + item.Name });
                }

                foreach (var item in await GetBooks())
                {
                    model.Books.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });
                }

                model.PersanalDataId = book.PersonalDataId;
                return View("Edit", model);
            }

            return RedirectToAction("Index", new Notice
            { TypeNotice = TypeNoticeEnum.danger, Message = "По непонятным причинам запись не была найдена! УЧИТЕЛЬ ДРОЙДЕКИ!!!" });
        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DetailsRentalBook model)
        {
            try
            {
                if (model.DateRental > model.DateRentalBefore)
                    ModelState.AddModelError("", "Дата выдачи должна быть раньше чем дата запланированного возврата");
                if (model.DateRental > model.DateReturn && model.DateReturn is not null)
                    ModelState.AddModelError("", "Дата выдачи должна быть раньше чем дата фактического возрата");

                if (ModelState.IsValid)
                {
                    Rental updateItem = new Rental
                    {
                        Id = model.Id,
                        BookId = model.BookId,
                        PersonalDataId = model.BookId,
                        DateRentalBefore = model.DateRentalBefore,
                        DateRental = model.DateRentalBefore,
                        DateReturn = model.DateReturn,
                    };
                    _context.Rentals.Update(updateItem);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Index", new Notice
                    { TypeNotice = TypeNoticeEnum.success, Message = "Изменение записи прошла успешна!" });
                }

                ModelState.AddModelError("", "Не все поля правильно заполнены!!!");

                foreach (var item in await GetPersonalData())
                {
                    model.PersonalData.Add(new SelectListItem
                    { Value = item.Id.ToString(), Text = item.Surname + " " + item.Name });
                }

                foreach (var item in await GetBooks())
                    model.Books.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Title });

                return View("Edit", model);
            }
            catch
            {
                return RedirectToAction("Index", new Notice
                { TypeNotice = TypeNoticeEnum.danger, Message = "Произошла ошибка при изменение записи!!!" });
            }
        }

        [Authorize(Roles = "Библиотекарь")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Rental itemRemove = new Rental { Id = id };
                _context.Rentals.Remove(itemRemove);
                await _context.SaveChangesAsync();
                Notice notice = new Notice();
                notice.TypeNotice = TypeNoticeEnum.success;
                notice.Message = "Удаление прошло успешно!!!";
                return RedirectToAction("Index", notice);
            }
            catch
            {
                Notice notice = new Notice();
                notice.TypeNotice = TypeNoticeEnum.danger;
                notice.Message = "Произошла ошибка при удаление записи!!!";
                return RedirectToAction("Index", notice);
            }

        }

        public async Task<IActionResult> ReternBook(int id)
        {
            try
            {
                Rental itemRetern = await GetRentalBook(id);
                Notice notice = new Notice();
                if (itemRetern != null)
                {
                    itemRetern.DateReturn = DateTime.Now;
                    await _context.SaveChangesAsync();
                    notice.TypeNotice = TypeNoticeEnum.success;
                    notice.Message = "Книга возвращена!";
                }
                else
                {
                    notice.TypeNotice = TypeNoticeEnum.warning;
                    notice.Message = "Книгу вернуть не получилось!";
                }
                return RedirectToAction("Index", notice);
            }
            catch
            {
                Notice notice = new Notice();
                notice.TypeNotice = TypeNoticeEnum.danger;
                notice.Message = "Произошла ошибка при удаление записи!!!";
                return RedirectToAction("Index", notice);
            }

        }

        private async Task<IEnumerable<Rental>> GetRentalBooks()
        {
            return await GetRentalBooks("");
        }
        private async Task<IEnumerable<Rental>> GetRentalBooks(string searchText)
        {
            if (!(User.Identity != null && User.Identity.IsAuthenticated))
                return new List<Rental>();
            if (User.IsInRole("Читатель"))
                return await GetRentalBooksSearchTitle(searchText);

            if (User.IsInRole("Библиотекарь"))
                return await GetRentalBooksSearchPersonalData(searchText);

            return new List<Rental>();
        }

        private async Task<IEnumerable<Rental>> GetRentalBooksSearchPersonalData(string searchText)
        {
            var personalData = await _context.PersonalData
                .Where(p => (p.Surname + " " + p.Name).Contains(searchText))
                .Select(d => d.Id)
                .ToListAsync();

            return await _context.Rentals
                .Where(d => personalData.Contains(d.PersonalDataId))
                .Include(d => d.Book)
                .Include(d => d.PersonalData)
                .ToListAsync();
        }
        private async Task<IEnumerable<Rental>> GetRentalBooksSearchTitle(string searchText)
        {
            int idPersonalData = await GetIdPersonalData();
            return await _context.Rentals
                .Include(d => d.Book)
                .Where(d => d.Book.Title.Contains(searchText) && d.PersonalDataId == idPersonalData)
                .Include(d => d.PersonalData)
                .ToListAsync();
        }

        private async Task<IEnumerable<PersonalData>> GetPersonalData()
        {
            return await _context.PersonalData
                .Include(p => p.User)
                .Include(p => p.User.Role)
                .Where(p => p.User.Role.Name == "Читатель")
                .ToListAsync();
        }
        private async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Rental> GetRentalBook(int id)
        {
            return await _context.Rentals
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


    }
}
