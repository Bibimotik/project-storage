using System.Reflection;

using Mapster;

using MapsterMapper;

using Storage.Application.Handlers.Users;

namespace Storage.API.Extensions;

public static class ApiExtensions
{
	public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
	{
		var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
		typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

		var mapperConfig = new Mapper(typeAdapterConfig);
		services.AddSingleton<IMapper>(mapperConfig);

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetUserByFilterQueryHandler>());

		return services;
	}
}
