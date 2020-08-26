using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SadWave.Events.Api.Common.Authentication;
using SadWave.Events.Api.Models.Authentication;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/authentication")]
	public class AuthenticationController : Controller
	{
		private readonly IAccountsService _accountsService;
		private readonly AuthenticationOptions _options;

		public AuthenticationController(IAccountsService accountsService, AuthenticationOptions options)
		{
			_accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		[HttpPost]
		public async Task<IActionResult> RequestToken([FromBody] TokenRequestBody requestBody)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			try
			{
				var account = await _accountsService.GetAsync(requestBody.Login, requestBody.Password);
				var credentials = new SigningCredentials(_options.Key, SecurityAlgorithms.HmacSha256);
				var claimsIdentity = ClaimsIdentityUtils.Create(account.Login, RoleUtils.GetName(account.Role));

				DateTime expiredDateTime;
				expiredDateTime = _options.TokenLifeTime.HasValue ?
					DateTime.UtcNow.Add(_options.TokenLifeTime.Value) :
					DateTime.MaxValue;

				var token = new JwtSecurityToken(
					issuer: _options.Issuer,
					claims: claimsIdentity.Claims,
					signingCredentials: credentials,
					notBefore: DateTime.UtcNow,
					expires: expiredDateTime);

				var handler = new JwtSecurityTokenHandler();
				var serializedToken = handler.WriteToken(token);

				return Ok(new
				{
					token = serializedToken,
					expires = expiredDateTime
				});
			}
			catch (AccountNotFoundException)
			{
				return Unauthorized();
			}
			catch (IncorrectPasswordException)
			{
				return Unauthorized();
			}
		}
	}
}
