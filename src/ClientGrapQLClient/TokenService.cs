using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ClientGrapQLClient
{
    public class TokenService
    {

        public TokenService()
        {
           
        }

        public string GenerateToken()
        {
            // Retrieve the key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("85495E41-EE8C-4DC8-BC49-E9C2C356EA75"));

            // Define the credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims (payload data for the token)
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, "vnada"),
            new Claim(JwtRegisteredClaimNames.Email, "vnadar@gmail.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
            new Claim("AggregratorId", "159") // Unique identifier for the token

        };

            // Generate the token
            var token = new JwtSecurityToken(
                issuer: "nupapp.com", // Set to null since you're not validating the issuer
                audience: "NUPADMINAPI", // Set to null since you're not validating the audience
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Token expiration time
                signingCredentials: credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
