using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Analogix_Backend_App.Presentation.WebAPI.Token
{
    // This class is intended to be a utility for handling token-related operations, such as generating and validating JWT tokens.  
    public class TokenTool
    {
        // ↓ Injection of tools to acess config files
        private readonly IConfiguration _config;

        public TokenTool(IConfiguration config)
        {
            _config = config;
        }

        
        public class Data // This class is intended to represent the data that will be included in the token, such as the member's ID and role.
        { 
        
            public required long MemberId { get; set; }
            public required string Role { get; set; }



        }

        // ↓ Method to generate a JWT token

        public string Generate(Data data) 
        {

            // ↓ Create claims based on the provided data
            Claim[] claims = [
                // ↓ Claim for the member's ID
                new Claim(ClaimTypes.NameIdentifier, data.MemberId.ToString()),
                // ↓ Claim for the member's role
                new Claim(ClaimTypes.Role, data.Role)
            ];

            // ↓ Creation of signing credentials using a secret key from the configuration
            string secret = _config["Token:Key"] ?? throw new Exception("Token secret not found in configuration."); // Ensure that the secret is not null
            byte[] key = Encoding.UTF8.GetBytes(secret); // Convert the secret key to a byte array
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key); // Create a symmetric security key using the byte array
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); // Create signing credentials using the security key and HMAC SHA-512 algorithm


            JwtSecurityToken token = new JwtSecurityToken(
                
                issuer: _config["Token:Issuer"], // Set the issuer of the token from the configuration
                audience: _config["Token:Audience"], // Set the audience of the token from the configuration
                claims: claims, // Include the claims in the token
                signingCredentials: credentials
            );

            // ↓ Return the generated token as a string
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token); // Convert the token to a string and return it



        }


    }
}
