using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.BAL;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    public partial class LoginController
    {
        /// <summary>
        /// "Autologin" - Login mittels Token
        /// </summary>
        /// <response code = "200">Success. DTOCustomer model im body</response>
        /// <response code = "401">Token fehlt oder nicht authorisiert</response>
        /// <response code = "403">Token entspricht nicht den Richtlinien (zB Rolle)</response>
        /// <response code = "400">Mit dem token wurde kein user gefunden obwohl er valid wäre(?)</response>
        /// <response code = "500">Server-Error. Mayday Mayday</response>
        [HttpGet]
        [Authorize(Roles = "MamaBaker")]
        public async Task<ActionResult<DTOCustomer>> LoginWithToken()
        {
            // wir versuchen anhand des tokens herauszufinden,
            // wer der User ist.
            string userMail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Nun suchen wir in der DB den passenden user
            Customer relevantUser =
                await _context.Customers
                .Include(x => x.Rents)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Email == userMail);

            // Sollte, aus welchem Grund auch immer,
            // der user nicht gefunden werden, dann -> BadRequest
            if (relevantUser == null)
                return BadRequest();

            DTOCustomer customer = _mapper.Map<DTOCustomer>(relevantUser);
            return Ok(customer);
        }
    }
}
