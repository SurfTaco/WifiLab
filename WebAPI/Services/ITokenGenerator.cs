using System;
namespace WebAPI.Services
{
    public interface ITokenGenerator
    {
        public string GetToken(string email, string role);
    }
}
