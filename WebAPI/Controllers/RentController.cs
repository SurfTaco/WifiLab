using System;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "MamaBaker")]
    [Route("api/[controller]/[action]")]
    public partial class RentController : BaseController
    {
        public RentController(LibContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }



}
