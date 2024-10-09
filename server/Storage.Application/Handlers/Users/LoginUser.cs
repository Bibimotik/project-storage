using MediatR;

using Storage.Domain.Interfaces;
using Storage.Domain.Models;

namespace Storage.Application.Handlers.Users;

public class LoginUserQuery(EntityModel user, string password) : IRequest
{
	public EntityModel User { get; } = user;
	public string Password { get; } = password;
}

public class LoginUserQueryHandler(IPasswordHash passwordHash) : IRequestHandler<LoginUserQuery>
{
	private readonly IPasswordHash _passwordHash = passwordHash;

	public Task Handle(LoginUserQuery request, CancellationToken cancellationToken)
	{
		var isCorrectPassword = _passwordHash.Verify(request.Password, request.User.Password);

		if (!isCorrectPassword)
			throw new UnauthorizedAccessException("Неверный пароль!");

		return Task.CompletedTask;
	}
}