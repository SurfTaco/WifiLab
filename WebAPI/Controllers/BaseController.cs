using System;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly LibContext _context;
        protected readonly IMapper _mapper;

        protected BaseController(LibContext context, IMapper mapper)
            => (_context, _mapper) = (context, mapper);
    }
}