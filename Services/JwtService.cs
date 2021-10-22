using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_auth.Services
{
    public class JwtService
    {
        private string securityKey = "_jaAyEMSJJhCRaNs4xuSFfdrb7hcWn7qpXa5jH7car60p5hPXAxSvlKd3qv3ZwK853R8NYgHjYqMezUy91Q3TwW5zkx6rn_hxW-HaX-naFezjzrRqP";

        public string Generate(int id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));

            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }
    }
}
