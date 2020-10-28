using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        [BindProperty]
        public Book Book { get; set; }
        public BooksController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _dbContext.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbContext.Books.Remove(bookFromDb);
            await _dbContext.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful." });
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                //create
                return View(Book);
            }
            //update
            Book = _dbContext.Books.FirstOrDefault(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    //create
                    _dbContext.Books.Add(Book);
                }
                else
                {
                    //update
                    _dbContext.Books.Update(Book);
                }

                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }
        #endregion
    }
}