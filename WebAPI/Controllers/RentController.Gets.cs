using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.BAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class RentController
    {
        /// <summary>
        /// Call um ein Buch auszuleiben
        /// </summary>
        /// <param name="bookID">die Id des auszuleihenden books</param>
        /// <response code="200">ok, im body kommt die aktuelle history des kunden mit</response>
        /// <response code="401">Kein valider Token dabei</response>
        /// <response code="409">Token ist angekommen, aber mit ihm wurde kein entsprechender user gefunden</response>
        /// <response code="400">Das Buch ist mittlerweile nicht mehr verfügbar oder das Buch gibt es nicht</response>
        /// <response code="500">Server-Error. Mayday</response>
        [HttpGet] 
        public async Task<ActionResult<DTORent[]>> RentBook(int bookID)
        {
            if (bookID == 0)
                return BadRequest();

            // TODO: Refactoring auslagern
            // useremail vom token rausfiltern
            string userMail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (String.IsNullOrEmpty(userMail))
                return Conflict();

            // find userId from DB
            var userID = await _context.Customers
                .Where(x => x.Email == userMail)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            //TODO: Refactor - auslagern - code reuse
            if (userID == 0)
                return Conflict("fehler bei token");

            // TODO: Refactor - auslagern - code reuse
            // TODO: Refactor - Datenbankeffizienz
            // final check ob das Buch immer noch verfügbar ist
            var book = await _context.Books
                    .Include(x => x.Rents)
                    .FirstOrDefaultAsync(x => x.Id == bookID);

            bool bookIsNotAvailable = book.Rents.Any(x => x.DateOfReturn == null);

            if (bookIsNotAvailable)
                return BadRequest("Buch doch schon verliehen oder gar nicht vorhanden.");

            // Wenn wir hier sind ist alles ok
            // neuen Rent erstellen
            Rent newRent = new()
            {
                BookId = bookID,
                CustomerId = userID,
                DateOfRent = DateTime.Now
            };

            // Jetzt adden und speichern wir den neuen Eintrag in der DB
            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();

            // jetzt holen wir uns einen aktuellen auszug der history
            // des users, damit er am Client immer die aktuellen Daten
            // hat. Sprich: Am Client können wir dann den Body
            // entgegenenehmen und einfach sagen History = DTO-Array im Body

            // TODO: Datenbankeffizienz -> unnötiger Datenverkehr
            // überarbeiten -> Client hat bereits alle Daten
            // Book und Author include ist unnötig.
            // hier holen wir uns mal alle Rents des Kunden raus
            Rent[] rentsOfUser =
                await _context.Rents
                .Include(x => x.Book)
                .ThenInclude(x => x.Author)
                .Where(x => x.CustomerId == userID)
                .ToArrayAsync();

            // wandeln sie in das dto model
            var dto = _mapper.Map<DTORent[]>(rentsOfUser);

            // und returnen die ganze sache mit einem Statuscode 200 OK
            return Ok(dto);
        }

        /// <summary>
        /// Call um ein Buch zu returnen
        /// </summary>
        /// <param name="rentId">die entsprechende RENT ID der Ausleihe</param>
        /// <response code="200">OK, returned wird das Datetime dateofReturn, denn somit ist
        /// das Model beim Client ident zur DB</response>
        /// <response code="401">Kein valider Token</response>
        /// <response code="400">Rent mit der übermittelten ID nicht vorhanden</response>
        /// <response code="500">Server-Error. Mayday</response>
        [HttpGet]
        public async Task<ActionResult<DateTime>> ReturnBook(int rentId)
        {
            // wir suchen das relevante rent aus der DB
            // und gleichzeitig ist dies auch ein prevent
            // um ein Buch nicht zweimal returnen zu können.
            var rent = await _context.Rents.FirstOrDefaultAsync
                (x => x.Id == rentId && x.DateOfReturn == null);

            // Sollte es null sein - also nicht vorhanden - Bad Request
            if (rent == null)
                return BadRequest();

            // sollte es vorhanden sein: Setzen wir das Rueckgabedatum auf datetime.now
            // und speichern die Änderungen in der Datenbank
            rent.DateOfReturn = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(rent.DateOfReturn);
        }
    }

}
