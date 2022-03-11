using System;

namespace DAL.BAL
{
    public class Rent
    {
        public int Id { get; set; }

        // Darstellung der N:M Beziehung zwischen
        // Books und Customers
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // Props
        public DateTime DateOfRent { get; set; }
        public DateTime? DateOfReturn { get; set; }
    }
}
