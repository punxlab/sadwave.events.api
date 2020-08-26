using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SadWave.Events.Api.Models.Account;
using SadWave.Events.Api.Services.Accounts;
using SadWave.Events.Api.Services.Exceptions;

namespace SadWave.Events.Api.Controllers
{
	[Route("api/account")]
	public class AccountController : Controller
	{
		private readonly IAccountsService _accountsService;

		public AccountController(IAccountsService accountsRepository)
		{
			_accountsService = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
		}

		[HttpPost]
		[Authorize(Roles = RoleName.Admin)]
		public async Task<IActionResult> Create([FromBody] CreateAccountRequestBody body)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest();

				await _accountsService.CreateAsync(body.Login, body.Password, Role.User);

				return Ok();
			}
			catch (AccountAlreadyExistsException)
			{
				return BadRequest();
			}
			catch (IncorrectPasswordException)
			{
				return BadRequest();
			}
		}
	}
}
