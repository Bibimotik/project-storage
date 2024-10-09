using MapsterMapper;

using MediatR;

using Storage.Domain.Abstractions;
using Storage.Domain.DTOs;
using Storage.Domain.Interfaces;
using Storage.Domain.Interfaces.Repositories;
using Storage.Domain.Models;

namespace Storage.Application.Handlers.Users;

public class UserRegistrationCommand<T>(string firstName,
									 string secondName,
									 string thirdName,
									 string phone,
									 string email,
									 string password,
									 EntityType type,
									 byte[] logo) : IRequest<T> where T : UserDto
{
	public string FirstName { get; set; } = firstName;
	public string SecondName { get; set; } = secondName;
	public string ThirdName { get; set; } = thirdName;
	public string Phone { get; set; } = phone;
	public string Email { get; set; } = email;
	public string Password { get; set; } = password;
	public EntityType Type { get; set; } = type;
	public byte[] Logo { get; set; } = logo ?? [];
}

public class UserRegistrationCommandHandler<T>(IEntityRepository entitiesRepository, IPasswordHash passwordHash, IMapper mapper) : IRequestHandler<UserRegistrationCommand<T>, UserDto> where T : UserDto
{
	private readonly IEntityRepository _entitiesRepository = entitiesRepository;
	private readonly IPasswordHash _passwordHash = passwordHash;
	private readonly IMapper _mapper = mapper;

	public async Task<UserDto> Handle(UserRegistrationCommand<T> request, CancellationToken cancellationToken)
	{
		// TODO - возможно лучше разбить Get На отдельные для User и Company чтобы потом не разбираться а какой тип юзера мы получили вообще
		var existUser = await _entitiesRepository.Get(request.Email, cancellationToken);

		if (existUser != null)
			throw new InvalidOperationException($"Пользователь с email {request.Email} уже существует.");

		EntityModel newUser = new(Guid.NewGuid(),
							request.FirstName,
							request.SecondName,
							request.ThirdName,
							request.Phone,
							request.Email,
							_passwordHash.Generate(request.Password),
							request.Type,
							request.Logo);

		await _entitiesRepository.Create(newUser, cancellationToken);

		return _mapper.Map<UserDto>(newUser);
	}
}
