using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Storage.Application.Handlers.Users;
using Storage.Domain.DTOs;

namespace Storage.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient<IRequestHandler<UserRegistrationCommand<UserDto>, UserDto>, UserRegistrationCommandHandler<UserDto>>();

		return services;
	}
}
