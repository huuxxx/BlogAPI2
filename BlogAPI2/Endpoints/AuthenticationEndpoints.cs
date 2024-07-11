using BlogAPI2.Contracts;
using BlogAPI2.Entities;
using BlogAPI2.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI2.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticantionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("login", async (LoginRequest request, UserManager<User> userManager, ConfigurationHelper configurationHelper) =>
            {
                var user = await userManager.FindByNameAsync(request.UserName);

                if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationHelper.GetJwtSecret()));

                    var token = new JwtSecurityToken(
                        issuer: configurationHelper.GetJwtIssuer(),
                        audience: configurationHelper.GetJwtAudience(),
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Results.Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                    });
                }

                return Results.Unauthorized();
            });

            app.MapPost("register", async (RegisterRequest request, UserManager<User> userManager) =>
            {
                var userExists = await userManager.FindByNameAsync(request.Username);

                if (userExists != null)
                    return Results.BadRequest("User already exists");

                User user = new User()
                {
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username
                };

                var result = await userManager.CreateAsync(user, request.Password);
                
                if (!result.Succeeded)
                    return Results.BadRequest("Failed to create user");

                return Results.Ok();
            })
            .RequireAuthorization();
        }
    }
}
