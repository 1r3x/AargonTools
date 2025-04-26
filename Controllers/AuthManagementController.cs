using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AargonTools.Configuration;
using AargonTools.Data;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Models;
using AargonTools.Models.DTOs.Requests;
using AargonTools.Models.DTOs.Responses;
using AargonTools.Manager.GenericManager;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    [Produces("application/json")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly ApiDbContext _apiDbContext;
        private readonly IUserService _userService;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParams,
            ApiDbContext apiDbContext, IUserService userService)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParams;
            _apiDbContext = apiDbContext;
            _userService = userService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Register_Hide_Out")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Register_Hide_Out([FromBody] UserRegistrationDto user)
        {
            if (!ModelState.IsValid)
            {
                Serilog.Log.Warning("Register_Hide_Out ----[Invalid payload]");
                return BadRequest(new RegistrationResponse
                {
                    Errors = new List<string> { "Invalid payload" },
                    Success = false
                });
            }

            Serilog.Log.Information("Register_Hide_Out => POST[{ClientIp}]--> {Email}", _userService.GetClientIpAddress(), user.Email);

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                Serilog.Log.Warning("Register_Hide_Out ----[Email already in use]");
                return BadRequest(new RegistrationResponse
                {
                    Errors = new List<string> { "Email already in use" },
                    Success = false
                });
            }

            var newUser = new IdentityUser { Email = user.Email, UserName = user.Username };
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                //default role user added 
                await _userManager.AddToRoleAsync(newUser, "User");

                Serilog.Log.Warning("Register_Hide_Out ----[User created successfully]");
                var jwtToken = await GenerateJwtToken(newUser);
                return Ok(jwtToken);
            }
            else
            {
                Serilog.Log.Warning("Register_Hide_Out ----[User creation failed]");
                return BadRequest(new RegistrationResponse
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });
            }
        }

        /// <summary>
        ///  Can generate a token and a refresh token.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can generate token and refresh token.
        /// It can be used for authentication process or request for a new token using refresh token.
        /// **Notes**
        /// Email and password both are required .
        /// </remarks>
        /// <response code="200">Successful request.</response>
        /// <response code="400">Something went wrong</response>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(RegistrationResponse), 200)]
        [ProducesResponseType(typeof(LoginErrorResponse), 400)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            Serilog.Log.Information("Login => POST[" + _userService.GetClientIpAddress() + "]--> " + user.Email);


            // Check if the client is still connected
            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                Serilog.Log.Warning("Client disconnected before processing");
                return StatusCode(StatusCodes.Status408RequestTimeout);
            }


            if (!ModelState.IsValid)
            {
                Serilog.Log.Warning("Invalid payload for user: " + user.Email);
                return BadRequest(new RegistrationResponse
                {
                    Errors = new List<string> { "Invalid payload" },
                    Success = false
                });
            }

            try
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    Serilog.Log.Warning("User not found: " + user.Email);
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string> { "Invalid login request, password or user doesn't match" },
                        Success = false
                    });
                }

                if (!await _userManager.CheckPasswordAsync(existingUser, user.Password))
                {
                    Serilog.Log.Warning("Invalid password for user: " + user.Email);
                    return BadRequest(new RegistrationResponse
                    {
                        Errors = new List<string> { "Invalid login request, password or user doesn't match" },
                        Success = false
                    });
                }

                var jwtToken = await GenerateJwtToken(existingUser);
                Serilog.Log.Information("User logged in successfully: " + user.Email);
                return Ok(jwtToken);
            }

            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while logging in user: " + user.Email);
                throw;
            }
        }




        /// <summary>
        /// Can refreshing a token 
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        ///  Refresh a expired tokens carry the information necessary to get a new access token.
        /// In other words, whenever an access token is required to access a specific resource,
        /// a client may use a refresh token to get a new access token issued by the authentication server.
        /// **Notes**
        /// Previous Token and previous RefreshToken both are required .
        /// </remarks>
        /// <response code="200">Successful request</response>
        /// <response code="400">Something went wrong</response>
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(AuthResult), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Invalid tokens"
                        },
                        Success = false
                    });
                }

                return Ok(result);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }


        private async Task<AuthResult> GenerateJwtTokenOld(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(50), //50 min now we should change it in production (5-10) min. 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private async Task<AuthResult> GenerateJwtTokenOld2(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(100), //expiration time increased to 100 min
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6), // todo Use a configuration value
                Token = $"{RandomString(35)}{Guid.NewGuid()}"
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);


            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,  // Add this
                Audience = _jwtConfig.Audience  // Add this
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = $"{RandomString(35)}{Guid.NewGuid()}"
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }


        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has not yet expired"
                        }
                    };
                }

                // validation 4 - validate existence of the token
                var storedToken = await _apiDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                // update current token 

                storedToken.IsUsed = true;
                _apiDbContext.RefreshTokens.Update(storedToken);
                await _apiDbContext.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };

                }
                else
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}