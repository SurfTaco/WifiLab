using System;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public partial class LoginController : BaseController
    {
        ITokenGenerator _tokenGenerator;

        public LoginController(LibContext context, IMapper mapper,
            ITokenGenerator tokengen) : base(context, mapper)
        {
            _tokenGenerator = tokengen;
        }
    }
}
