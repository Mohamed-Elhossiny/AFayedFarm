using AFayedFarm.Dtos.Auth;
using AFayedFarm.Model;
using AFayedFarm.Model.AuthModels;
using System.IdentityModel.Tokens.Jwt;

namespace AFayedFarm.Repositories.Auth
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterDto model);
		Task<AuthModel> LoginAsync(LoginDto model);
		Task<JwtSecurityToken> CreateToken(ApplicationUser user);
	}
}
