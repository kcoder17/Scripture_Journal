using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScriptureJournal.Models;

namespace ScriptureJournal.Pages.JournalEntries
{
    public class IndexModel : PageModel
    {
        private readonly ScriptureJournal.Models.ScriptureJournalContext _context;

        public IndexModel(ScriptureJournal.Models.ScriptureJournalContext context)
        {
            _context = context;
        }

        public IList<JournalEntry> JournalEntry { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Books { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Book { get; set; }

        public async Task OnGetAsync()
        {
            // Use LINQ to get list of books.
            IQueryable<string> bookQuery = from m in _context.JournalEntry
                                            orderby m.Book
                                            select m.Book;

            var entries = from m in _context.JournalEntry
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                entries = entries.Where(s => s.Note.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(Book))
            {
                entries = entries.Where(x => x.Book == Book);
            }

            Books = new SelectList(await bookQuery.Distinct().ToListAsync());
            JournalEntry = await entries.ToListAsync();
        }
    }
}
