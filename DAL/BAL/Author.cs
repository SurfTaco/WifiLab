using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.BAL
{
    public class Author
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Surname { get; set; }

        // 1:N zu Books
        public ICollection<Book> Books { get; set; }
    }
}
