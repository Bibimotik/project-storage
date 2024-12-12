using Microsoft.Extensions.DependencyInjection;

using Storage.Domain.Interfaces;

namespace Storage.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHash, PasswordHash>();

		return services;
	}
}
