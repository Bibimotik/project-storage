using Microsoft.AspNetCore.Diagnostics;

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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExceptionHandler(config =>
{
	config.Run(async context =>
	{
		var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
		var exception = exceptionHandler?.Error;

		if (exception != null)
		{
			var handler = app.Services.GetRequiredService<GlobalExceptionHandler>();
			await handler.TryHandleAsync(context, exception, CancellationToken.None);
		}
	});
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
