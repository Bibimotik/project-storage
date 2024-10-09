using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Storage.Domain.Interfaces;
using Storage.Domain.Interfaces.Repositories;
using Storage.Persistance.Repositories;

namespace Storage.Persistance;

public static class PersistenceExtensions
{
	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		string? connectionString = configuration.GetConnectionString("POSTGRESQL__DEV");

		Debug.WriteLine("--------- " + connectionString);

		if (connectionString == null)
			throw new NullReferenceException("Ошибка обращения к БД!");

		services.AddScoped<IStorageDBContext>(provider => new StorageDBContext(connectionString));

		services.AddScoped<IEntityRepository, EntityRepository>();

		return services;
	}
}