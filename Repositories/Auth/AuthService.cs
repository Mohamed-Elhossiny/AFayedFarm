using AFayedFarm.Dtos.Auth;
using AFayedFarm.Enums;
using AFayedFarm.Helper;
using AFayedFarm.Model;
using AFayedFarm.Model.AuthModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AFayedFarm.Repositories.Auth
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly JWT jwt;

		public AuthService(UserManager<ApplicationUser> userManager,IOptions<JWT> jwt)
        {
			this.userManager = userManager;
			this.jwt = jwt.Value;
		}

		public async Task<AuthModel> RegisterAsync(RegisterDto model)
		{
			if (await userManager.FindByEmailAsync(model.Email) != null)
				return new AuthModel { Message = "Email is already registerd!",IsAuthenticated = false };
			if (await userManager.FindByNameAsync(model.UserName) != null)
				return new AuthModel { Message = "UserName is already registerd",IsAuthenticated = false };
			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
				FName = model.Name,
			};
			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += $"{error.Description},";
				}
				return new AuthModel { Message = errors,IsAuthenticated = false };
			}

			await userManager.AddToRoleAsync(user, UserRoleEnum.User.ToString());
			var roles = await userManager.GetRolesAsync(user);

			var jwtSecurityToken = await CreateToken(user);

			return new AuthModel
			{
				IsAuthenticated = true,
				UserName = user.UserName,
				Email = user.Email,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpiresOn = jwtSecurityToken.ValidTo,
			};
		}

		public async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
		{
			// Get Roles and Create Claims to store in the Token
			var roles = await userManager.GetRolesAsync(user);
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name,user.FName!),
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(JwtRegisteredClaimNames.Email,user.Email!),
			};
			foreach (var item in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, item));
			}

			// Create Security Key
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
			var signingCridentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			// Create Token
			var token = new JwtSecurityToken(
				issuer: jwt.Issuer,
				audience: jwt.Audiance,
				claims: claims,
				expires: DateTime.Now.AddDays(jwt.Duration),
				signingCredentials : signingCridentials
				);

			return token;
		}

		public async Task<AuthModel> LoginAsync(LoginDto model)
		{
			var authModel = new AuthModel();

			var user = await userManager.FindByEmailAsync(model.Email);

			if(user is null || !await userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "Email or Password is incorrect";
				return authModel;
			}
			var jwtSecuirtyToken = await CreateToken(user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecuirtyToken);
			authModel.Email = user.Email!;
			authModel.UserName = user.UserName!;
			authModel.ExpiresOn = jwtSecuirtyToken.ValidTo;

			return authModel;
		}

		
	}
}
