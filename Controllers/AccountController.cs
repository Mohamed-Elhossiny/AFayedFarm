using AFayedFarm.Dtos.Auth;
using AFayedFarm.Repositories.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAuthService service;

		public AccountController(IAuthService service)
		{
			this.service = service;
		}

		[HttpPost("~/Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await service.RegisterAsync(model);
			if(!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}

		[HttpPost("~/Login")]
		public async Task<IActionResult> Login([FromBody] LoginDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await service.LoginAsync(model);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);	
		}
	}
}
