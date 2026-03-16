using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using Analogix_Backend_App.Presentation.WebAPI.Dto;
using Analogix_Backend_App.Presentation.WebAPI.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Analogix_Backend_App.Domain.BusineesExceptions.UserException;

namespace Analogix_Backend_App.Presentation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService; // Allows to communicate with the user service for authentication operations.
        private readonly TokenTool _tokenTool; // This line declares a private readonly field of type TokenTool, which will be used to generate and manage authentication tokens for users.


        // Allows for dependency injection of the IUserService, enabling the controller to use the service's methods for handling authentication-related requests.
        public AuthenticationController(IUserService userService, TokenTool tokenTool)
        {
            _userService = userService; // Constructor injection of the IUserService dependency.
            _tokenTool = tokenTool; // Constructor injection of the TokenTool dependency.
        }

        [HttpPost("Register")] // This attribute specifies that this action method will handle HTTP POST requests to the "register" endpoint.
        public IActionResult Register([FromBody] AuthRegisterRequestDto dto)
        {
            var role = UserRoles.User; // Assigns the default role of "User" to the new user being registered.

            User user = new User
            (
                dto.Username,
                dto.Password,
                dto.Email,
                role

            );

            _userService.Register(user);

            return Ok("User registered successfully."); // Returns a 200 OK response with a success message upon successful registration.

        }

        [HttpPost("Login")] // This attribute specifies that this action method will handle HTTP POST requests to the "login" endpoint.
        public IActionResult Login([FromBody] AuthLoginRequestDto dto)
        {
            // in this case ModelState.IsValid is used to check if the incoming data in the dto (Data Transfer Object) is valid according to the defined validation rules.
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            // Calls the Login method of the IUserService to authenticate the user with the provided email and password.
            User user = _userService.Login(dto.Email, dto.Password);

            try
            {
                string token = _tokenTool.Generate(new TokenTool.Data()
                {
                    MemberId = user.Id, // Sets the MemberId property of the token data to the authenticated user's ID.
                    Role = user.Role.ToString() // Sets the Role property of the token data to the authenticated user's role, converted to a string.
                });


                return Ok(new
                {
                    Message = "Login successful.",
                    Token = token
                }); // Returns a 200 OK response with the authenticated user's information upon successful login.
            }
            catch (UserBadCredentialException ex)
            {
                return Unauthorized(new
                {
                    Message = ex.Message
                }); // Returns a 400 Bad Request response with an error message if the login process fails.




            }
        }
    }
}
