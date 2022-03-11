using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.BAL
{
    public class Customer
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Surname { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsAdmin { get; set; }

        // 1:1 Credentials
        public Credential Credential { get; set; }

        // N:M zu Books
        public ICollection<Rent> Rents { get; set; }
    }
}
