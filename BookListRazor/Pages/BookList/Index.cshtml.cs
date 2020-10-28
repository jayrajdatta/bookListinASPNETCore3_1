using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dBcontext;
        public IndexModel(ApplicationDbContext dBcontext)
        {
            _dBcontext = dBcontext;
        }

        public IEnumerable<Book> Books { get; set; }
        public async Task OnGet()
        {
            Books = await _dBcontext.Book.ToListAsync();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var bk = await _dBcontext.Book.FindAsync(id);

            if(bk == null)
            {
                return NotFound();
            }

            _dBcontext.Book.Remove(bk);
            await _dBcontext.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}