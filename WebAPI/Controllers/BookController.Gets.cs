using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.BAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class BookController
    {
        /// <summary>
        /// Das Abrufen von allen momentan verfügbaren (ergo nicht ausgeliehenen) Büchern
        /// </summary>
        /// <response code="200">Im Body die frei verfügbaren Bücher</response>
        /// <response code="401">Token nicht valid oder nicht mitgesendet</response>
        /// <response code="500">Server-Error! Vielleicht DB-Offline?</response>
        [HttpGet]
        public async Task<ActionResult<DTOBook[]>> GetAvailableBooks()
        {
            var allBooks = await
                _context.Books
                .Include(x => x.Rents) // inklusive die Rents-Objekte
                .Include(x => x.Author) // inklusive die Author-Objekte
                .Where(x => x.Rents.All(x => x.DateOfReturn != null))
                .ToArrayAsync();

            // Mapper reinhängen -> Book -> BookDTO
            var mapped = _mapper.Map<DTOBook[]>(allBooks);

            return Ok(mapped);
        }
    }
}
