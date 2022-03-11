using System;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class LoginController
    {
        /// <summary>
        /// Login Call mit Benutzerdaten email und pass
        /// </summary>
        /// <returns>einen Token und einen DTOCustomer als Tuple</returns>
        /// <response code="400">bad user data</response>
        /// <response code="200">OK, returned Tuple(item1: token als string,
        /// item2: DTOCustomer model</response>
        /// <response code="500">Server-Error. Mayday</response>
        [HttpPost]
        public async Task<ActionResult<Tuple<string, DTOCustomer>>> LoginWithData
            ([FromHeader] string email, [FromHeader] string pass)
        {
            // Falls die erforderlichen DAten im HEader
            // schon gar nicht übermittelt wurden
            // können wir gleich einen BadRequest zurückgeben.
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(pass))
                return BadRequest();

            // TODO: Delete toLower nachdem wir ToLower fix in die DB speichern
            // Verwende Email um User zu finden
            var user = await _context.Customers
                .Include(x => x.Credential)
                .Include(x => x.Rents)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            // wenn kein user mit email gefunden wurde, return 400er
            if (user == null)
                return BadRequest("bad user data");

            // verify Passwort
            // Wir holen das Pass aus dem Model heruas
            var passFromDB = user.Credential.Password;
            // wir adden das Salz mit dem übermittelnten passwort
            var saltPass = user.RegistrationDate + pass;
            // Verify mit Bcrypt
            var result = BCrypt.Net.BCrypt.Verify(saltPass, passFromDB);

            // Wenn Passwort falsch, dann 400er
            if (!result)
                return BadRequest("bad user data");

            // Sollten wir hier noch leben, ist alles ok. User ist verifiziert
            // Token erstellen mit emailadresse (Username) und seiner Rolle
            string token = _tokenGenerator.GetToken(email.ToLower(), "MamaBaker");

            // jetzt wandeln wir den gefundenen user aus der db
            // in ein entsprechendes client datenmodell (unser dto)
            var dto = _mapper.Map<DTOCustomer>(user);

            return Ok(new Tuple<string, DTOCustomer>(token,dto));
        }
    }
}
