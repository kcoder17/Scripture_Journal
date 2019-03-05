using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptureJournal.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ScriptureJournalContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ScriptureJournalContext>>()))
            {
                // Look for any movies.
                if (context.JournalEntry.Any())
                {
                    return;   // DB has been seeded
                }

                context.JournalEntry.AddRange(
                    new JournalEntry
                    {
                        Date = DateTime.Parse("2019-2-12"),
                        Book = "First Nephi",
                        Note = "Test entry"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
