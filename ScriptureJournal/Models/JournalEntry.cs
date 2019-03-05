using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptureJournal.Models
{
    public class JournalEntry
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(20)]
        [Required]
        public string Book { get; set; }

        [Required]
        public string Note { get; set; }

    }
}
