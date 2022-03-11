using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.BAL
{
    public class Book
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        [MaxLength(200)]
        public Uri CoverPicture { get; set; }

        // 1:N zu Author
        public int AuthorID { get; set; }
        public Author Author { get; set; }

        // N:M zu Books
        public ICollection<Rent> Rents { get; set; }

    }
}
