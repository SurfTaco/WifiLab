using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.BAL;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class CustomerController
    {
        /// <summary>
        /// Hier kannst du den Customer registrieren
        /// </summary>
        /// <param name="newCustomer">Collection History kann null oder leeres Array sein</param>
        /// <param name="password">one lower; one upper; one special; one number; 8 chars min</param>
        /// <response code="409">Email-adresse bereits vergeben</response>
        /// <response code="400">Passwort stimmt nicht mit Richtlinie überein oder Übermittlungsfehler des Models</response>
        [HttpPost]
        public async Task<ActionResult> RegisterCustomer(DTOCustomer newCustomer,
            [FromHeader] string password)
        {
            if (String.IsNullOrEmpty(password) || newCustomer == null)
                return BadRequest();

            // Ist Email bereits vorhanden?
            if (await EmailAlreadyRegistered(newCustomer.Email))
                return Conflict("Email-Adresse bereits vorhanden");

            // Regex-Check ob Passwort stimmt
            if (!IsPassValid(password))
                return BadRequest();
            
            // Umwandlung vom DTO zu unserem BAL Model
            var customer = _mapper.Map<Customer>(newCustomer);

            // DAtetime now wird gesetzt (u.a. für den Hash)
            customer.RegistrationDate = DateTime.Now;

            customer.Credential = new Credential()
            {
                Password = GetHashedPassword(customer.RegistrationDate, password)
            };

            // TODO: Die Email-Adresse als toLower in die DB schreiben.

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Call um das Passwort zu ändern
        /// </summary>
        /// <param name="oldPass">das aktuelle Passwort</param>
        /// <param name="newPass">das neue Wunschpasswort</param>
        /// <response code="200">Success -> hat geklappt</response>
        /// <response code="400">neues Passwort entspricht nicht den Richtlinien</response>
        /// <response code="401">token nicht valid</response>
        /// <response code="403">Oldpass ist nicht das richtige aktuelle Kennwort</response>
        /// <response code="409">aus dem übermittelten Token konnte kein USer gefunden werde</response>
        [HttpPost]
        [Authorize(Roles = "MamaBaker")]
        public async Task<ActionResult> ChangePassword
            ([FromHeader] string oldPass, [FromHeader] string newPass)
        {
            if (!IsPassValid(newPass))
                return BadRequest();

            // identifizierung des users via token
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // aus der db den aktuellen userdatensatz holen
            Customer customer = await _context.Customers
                .Include(x => x.Credential)
                .FirstOrDefaultAsync(x => x.Email == email);

            // Sollte sich aus dem übermittelten Token kein USer finden
            // returnen wir Conflict 409.
            if (customer == null)
                return Conflict();

            // Altes Passwort auf seine Richtigkeit überprüfen
            // Wir verifyen es mit dem aktuellen Eintrag
            var result = BCrypt.Net.BCrypt.Verify
                (customer.RegistrationDate + oldPass, customer.Credential.Password);

            // Falls das aktuelle Pass nicht richtig übermittelt wurde
            // forbidden 403
            if (!result)
                return Forbid();

            // jetzt salten und hashen wir es
            string hashedpass = GetHashedPassword(customer.RegistrationDate, newPass);

            // jetzt holen wir uns das credential in eine eigene Variable
            Credential credential = customer.Credential;

            // ändern das Passwort
            credential.Password = hashedpass;
            // Und speichern die DB-Änderungen
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        bool IsPassValid(string pass)
        {
            return Regex.IsMatch(pass, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        }

        string GetHashedPassword(DateTime regDate, string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(regDate + pass);
        }
        
    }
}
