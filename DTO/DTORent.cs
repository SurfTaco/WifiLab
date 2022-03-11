using System;
namespace DTO
{
    public class DTORent
    {
        public int Id { get; set; }
        public DTOBook Book { get; set; }
        public DateTime DateOfRent { get; set; }
        public DateTime? DateOfReturn { get; set; }
    }
}
