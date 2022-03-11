using System;
namespace DTO
{
    public class DTOCustomer
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        public DTORent[] History { get; set; }
    }
}
