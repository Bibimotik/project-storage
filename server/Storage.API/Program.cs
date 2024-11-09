using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Storage.API.Extensions;
using Storage.API.Middlewares;
using Storage.Application;
using Storage.Infrastructure;
using Storage.Persistance;

var builder = WebApplication.CreateBuilder(args);
var services  = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddSwaggerGen();

services.AddExceptionHandler<GlobalExceptionHandler>();

services.AddProblemDetails();

services
	.AddAPI(configuration)
	.AddApplication()
	.AddInfrastructure()
	.AddPersistence(configuration);

var app = builder.Build();

app.UseExceptionHandler(config =>
{
	config.Run(async context =>
	{
		var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
		var exception = exceptionHandler?.Error;

		if (exception != null)
		{
			var handler = app.Services.GetRequiredService<GlobalExceptionHandler>();

			// Обрабатываем исключение и предотвращаем его дальнейшее выбрасывание
			bool handled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

			if (handled)
			{
				// Если исключение обработано, не выбрасываем его снова
				return;
			}
		}

		// Если исключение не обработано, можно выбросить его повторно, но лучше избегать этого
		throw exception;
	});
});

app.MapHealthChecks(
	"/health",
	new HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
//app.UseCors();

app.UseRouting();

app.MapControllers();

//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
