using Storage.API.Middlewares;

namespace Storage.API.Extensions;

public static class ExceptionHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
	{
		// Добавляем пользовательский middleware для глобальной обработки исключений
		return builder.UseMiddleware<GlobalExceptionHandler>();
	}
}
