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

        public string BookSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<JournalEntry> JournalEntry { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Books { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Book { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            BookSort = String.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

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

            switch (sortOrder)
            {
                case "book_desc":
                    entries = entries.OrderByDescending(s => s.Book);
                    break;
                case "Date":
                    entries = entries.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    entries = entries.OrderByDescending(s => s.Date);
                    break;
                default:
                    entries = entries.OrderBy(s => s.Book);
                    break;
            }
            int pageSize = 5;
            Books = new SelectList(await bookQuery.Distinct().ToListAsync());
            JournalEntry = await PaginatedList<JournalEntry>.CreateAsync(entries.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
