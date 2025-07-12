using ApplicationLayer.Contracts.Auth;
using ApplicationLayer.DTOs.Auth;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<LoginDTOResponse>>> LogIn(LoginRequestDTO loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            var result = await userManager.CheckPasswordAsync(user ,loginRequest.Password);

            if (!result)
                return APIResponse<LoginDTOResponse>.FailureResponse(401, null, "Wrong Email or Password");

            var response = new LoginDTOResponse()
            {
                Token = await tokenService.CreateTokenAsync(user, userManager)
            };

            var userRefreshToken = user.RefreshTokens?.FirstOrDefault(T => T.IsActive);

            if (string.IsNullOrEmpty(userRefreshToken?.Token))
            {
                var refreshToken = GenerateRefreshToken();

                user?.RefreshTokens?.Add(refreshToken);
                await userManager.UpdateAsync(user);

                SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);
            }
            else
            {
                SetRefreshTokenInCookie(userRefreshToken.Token, userRefreshToken.ExpiresOn);
            }

            return APIResponse<LoginDTOResponse>.SuccessResponse(200, response, "user logged in successfully");
        }

        [HttpGet("Refresh")]
        public async Task<ActionResult<APIResponse<LoginDTOResponse>>> Refresh()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return  StatusCode(401 ,APIResponse<LoginDTOResponse>.FailureResponse(401, null, "Log in and try again."));
            }

            //var user = userManager.Users.FirstOrDefault(U => U.RefreshTokens.Any(T => T.IsActive && T.Token == refreshToken));
            var user = userManager.Users.FirstOrDefault(U => U.RefreshTokens.Any(T => T.ExpiresOn > DateTime.Now && T.Token == refreshToken));

            if (user is null)
            {
                return NotFound(APIResponse<LoginDTOResponse>.FailureResponse(404, null, "Log in and try again."));
            }

            var loginResponse = new LoginDTOResponse()
            {
                Token = await tokenService.CreateTokenAsync(user, userManager)
            };

            return Ok(APIResponse<LoginDTOResponse>.SuccessResponse(200, loginResponse, "successfully"));
        }

        private RefreshToken GenerateRefreshToken()
        {
            var token = Guid.NewGuid().ToString();

            return new RefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"] ?? "30")),
                CreatedOn = DateTime.UtcNow
            };
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime ExpiresOn)
        {
            var cookiewOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = ExpiresOn.ToLocalTime(),
                SameSite = SameSiteMode.None,
                Secure = true
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookiewOptions);
        }
    
    }
}
