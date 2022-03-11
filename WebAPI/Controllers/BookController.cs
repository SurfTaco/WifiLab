using System;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "MamaBaker")]
    [Route("api/[controller]/[action]")]
    public partial class BookController : BaseController
    {
        public BookController(LibContext context, IMapper mapper) : base(context, mapper)
        {
            
        }
    }
}
