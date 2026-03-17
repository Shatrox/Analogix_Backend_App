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
        
            public required long UserId { get; set; }
            public required string Role { get; set; }



        }

        // ↓ Method to generate a JWT token

        public string Generate(Data data) 
        {

            // ↓ Create claims based on the provided data
            Claim[] claims = [
                // ↓ Claim for the member's ID
                new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
                // ↓ Claim for the member's role
                new Claim(ClaimTypes.Role, data.Role)
            ];

            // ↓ Creation of signing credentials using a secret key from the configuration
            string secret = _config["Token:Key"] ?? throw new Exception("Token secret not found in configuration."); // Ensure that the secret is not null
            byte[] key = Encoding.UTF8.GetBytes(secret); // Convert the secret key to a byte array
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key); // Create a symmetric security key using the byte array
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); // Create signing credentials using the security key and HMAC SHA-512 algorithm

            int expireMinutes = int.Parse(_config["Token:Expire"] ?? "60"); // Get the token expiration time from the configuration, defaulting to 60 minutes if not set
            DateTime utcNow = DateTime.UtcNow; // Get the current UTC time            

            JwtSecurityToken token = new JwtSecurityToken(
                
                issuer: _config["Token:Issuer"], // Set the issuer of the token from the configuration
                audience: _config["Token:Audience"], // Set the audience of the token from the configuration
                claims: claims, // Include the claims in the token
                notBefore: utcNow, // Set the token to be valid immediately
                expires: utcNow.AddMinutes(expireMinutes), // Set the token to expire after the specified number of minutes
                signingCredentials: credentials
            );

            // ↓ Return the generated token as a string
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token); // Convert the token to a string and return it



        }


    }
}
