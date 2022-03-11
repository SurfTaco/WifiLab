using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.BAL
{
    public class Credential
    {
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Password { get; set; }
        
        // 1:1 zu Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
