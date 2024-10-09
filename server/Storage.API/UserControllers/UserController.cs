using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Storage.API.Contracts.Users;
using Storage.Application.Handlers.Users;
using Storage.Domain.Abstractions;
using Storage.Domain.DTOs;

namespace Storage.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator, IMapper mapper) : ControllerBase
{
	private readonly IMediator _mediator = mediator;
	private readonly IMapper _mapper = mapper;

	[HttpPost(nameof(Login))]
	public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
	{
		var user = await _mediator.Send(new GetUserByFilterQuery(email: request.Email));

		if (user == null)
			throw new InvalidOperationException("Пользователь с такой почтой не существует.");

		await _mediator.Send(new LoginUserQuery(user, request.Password));

		return Ok();
	}

	[HttpPost(nameof(UserRegistration))]
	public async Task<IActionResult> UserRegistration([FromBody] CreateUserRegistrationRequest request)
	{
		if (!Enum.TryParse<EntityType>(request.Type, out var type))
			throw new InvalidOperationException("Такого типа пользователя не сущестувует.");

		if (type != EntityType.User)
			throw new InvalidOperationException("Роль не соответствует необходимой.");

		var user = await _mediator.Send(new UserRegistrationCommand<UserDto>(request.FirstName,
															  request.SecondName,
															  request.ThirdName,
															  request.Phone,
															  request.Email,
															  request.Password,
															  type,
															  request.Logo));

		return Ok(user);
	}

	[HttpPost(nameof(CompanyRegistration))]
	public async Task<IActionResult> CompanyRegistration([FromBody] CreateUserRegistrationRequest request)
	{
		if (!Enum.TryParse<EntityType>(request.Type, out var type))
			throw new InvalidOperationException("Такого типа пользователя не сущестувует.");

		if (type != EntityType.Company)
			throw new InvalidOperationException("Роль не соответствует необходимой.");

		var user = await _mediator.Send(new CompanyRegistrationCommand<CompanyDto>(request.FirstName,
															  request.SecondName,
															  request.ThirdName,
															  request.Phone,
															  request.Email,
															  request.Password,
															  type,
															  request.Logo));

		return Ok(user);
	}
}