using Microsoft.Extensions.DependencyInjection;

namespace Storage.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		return services;
	}
}
