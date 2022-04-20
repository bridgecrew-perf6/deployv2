
using System.Globalization;
using backend.Recycle.Extensions;
using backend.Recycle.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Recycle.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using backend.Recycle.Data.Models;
    using backend.Recycle.Data.Models.Identity;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    //
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly IConfiguration _config;
        private IEmailProvider _email;
        public IdentityController(UserManager<Users> userManager,
           IConfiguration config,IEmailProvider email)
        {
            this.userManager = userManager;
            _config = config;
            _email = email;
        }
        [Route(nameof(Register))]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterUserRequestModel model)
        {
            var user = new Users
            {
                Email = model.Email,
                UserName = model.UserName,
                SSID = model.SSID,
                Address = model.Address
            };
            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var role = await this.userManager.AddToRoleAsync(user, "user");

                var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.ActionLink("EmailConfirmation", "Identity",
                    new {token,email = user.Email}
                    );
               await _email.Send(user.Email, confirmationLink);

               return Ok();
            }
            return BadRequest(result.Errors);
        }
        [Route(nameof(Login))]
        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.UserName);

            if (user == null)
            {
                return NotFound();
            }
            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return NotFound();
            }

            var EmailConfirmed =await  userManager.IsEmailConfirmedAsync(user);
            if (!EmailConfirmed)
            {
                return BadRequest("Email not Confirmed Yet !");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Secret"]);
            var userRole = userManager.GetRolesAsync(user).Result.First();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),

                    new Claim("userId", user.Id.ToString()),
                    new Claim("Name",user.UserName),
                    new Claim("Email",user.Email),
                    new Claim(ClaimTypes.Role,userRole)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }
        [Route(nameof(EmailConfirmation))]
        [HttpGet]
        public async Task<ActionResult> EmailConfirmation(string token,string email)
        {
            var user =await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            var result =await  userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(new {errors = result.Errors});
            }

            return Ok("Confirmation Successfully");

        }
        [Route(nameof(ForgetPassword))]
        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ResetPassword model)
        {
            var result = await userManager.FindByEmailAsync(model.Email);
            if (result == null)
            {
                return NotFound("Email NotFound");
            }
            var token = await this.userManager.GeneratePasswordResetTokenAsync(result);
            var confirmationLink = Url.ActionLink("ResetPasswordConfirmation", "Identity",
                new { token, email = model.Email }
            );
            await _email.Send(model.Email, confirmationLink);
            return Ok(token);
        }

         

        [Route(nameof(ResetPasswordConfirmation))]
        [HttpPost]
        public async Task<ActionResult> ResetPasswordConfirmation(string token , string email,string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User NotFound");
            var result = await userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
        [Route(nameof(ResendEmailConfirmation)+"/{email}")]
        [HttpPost]
        public async Task<ActionResult> ResendEmailConfirmation(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            var confirmation = await userManager.IsEmailConfirmedAsync(user);
            if (confirmation)
            {
                return Ok("Email already Confirmed");
            }

            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.ActionLink("EmailConfirmation", "Identity",
                new { token, email = user.Email }
            );
            await _email.Send(user.Email, confirmationLink);
            return Ok(token);
        }
        [Route(nameof(GetUserData))]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserData()
        {
            var user = new
            {
                userId=User.GetUserId(),
                Email = User.GetUserEmail(),
                Name = User.GetUserName(),
            };
            return Ok(user);
        }

    }
}
