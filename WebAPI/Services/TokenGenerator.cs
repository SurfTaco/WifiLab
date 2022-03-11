using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GetToken(string email, string role)
        {
            return GenerateToken(email, role);
        }

        string GenerateToken(string email, string role)
        {
            var claims = new List<Claim>
            {
                // Den Usernamen / EMail "speichern"
                new Claim(JwtRegisteredClaimNames.Sub, email.ToLower()),
                // hier speichern wir "seinen Token" bzw. seine ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Registrieren wir den Token damit wir ihn beim usernamen ansprechen dürfen
                new Claim(ClaimTypes.NameIdentifier, email),
                // Wir registrieren seine Rolle
                new Claim(ClaimTypes.Role, role)
            };

            // Jetzt müssen wir den Key aus der config auslesen
            // und machen daraus einen SymmetricSecKey (Frameworkbasierend!)
            // Wichtig! Der SecurityKey muss als Bytes geliefert werden
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwTKey"]));

            // Hier entsteht sozusagen der Key
            // Wir verschlüssen die Credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // jetzt können wir noch das Expire hinzufügen
            // also: wann läuft der Token wieder ab
            var expire = DateTime.Now.AddDays(Convert.ToDouble(_config["JwtExpire"]));

            // Und Erstellen den Token
            JwtSecurityToken token = new JwtSecurityToken(
                _config["JwtIssuer"],
                _config["JwtIssuer"],
                claims,
                expires: expire,
                signingCredentials: cred
                );

            // Der Securityhanlder macht nun aus unserem Token
            // einen String und wir returnen zur "Hauptmethode"
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
