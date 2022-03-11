using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class CustomerController
    {
        /// <summary>
        /// Check ob eine E-MailAdresse bereits vergeben ist
        /// </summary>
        /// <returns>true wenn bereits vergebsen sonst false</returns>
        [HttpGet]
        public async Task<bool> EmailAlreadyRegistered(string mail)
        {
            return await _context.Customers.AnyAsync(x => x.Email.ToLower() == mail.ToLower());
        }
    }
}
