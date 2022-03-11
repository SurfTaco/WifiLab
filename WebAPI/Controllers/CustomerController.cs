using System;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public partial class CustomerController : BaseController
    {
        public CustomerController(LibContext context, IMapper mapper) : base(context, mapper)
        {
            
        }
    }
}
