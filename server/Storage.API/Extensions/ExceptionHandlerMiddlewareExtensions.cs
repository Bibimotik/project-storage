using Storage.API.Middlewares;

namespace Storage.API.Extensions;

public static class ExceptionHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseCustomExceptionHandler(this
		IApplicationBuilder builder)
	{
		return builder.UseMiddleware<GlobalExceptionHandler>();
	}
}