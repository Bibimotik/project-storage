using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Storage.Application.Exceptions;

using System.ComponentModel.DataAnnotations;

namespace Storage.API.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
	public GlobalExceptionHandler()
	{
	}

	private static readonly Dictionary<Type, (int StatusCode, string Title)> ExceptionMappings = new()
	{
		{ typeof(UnauthorizedAccessException), (StatusCodes.Status401Unauthorized, "Unauthorized Access") },
		{ typeof(ArgumentException), (StatusCodes.Status400BadRequest, "Invalid Argument") },
		{ typeof(NullReferenceException), (StatusCodes.Status500InternalServerError, "Null Reference Error") },
		{ typeof(InvalidOperationException), (StatusCodes.Status409Conflict, "Invalid Operation") },
		{ typeof(NotFoundException), (StatusCodes.Status404NotFound, "Not Found") },
		{ typeof(ValidationException), (StatusCodes.Status400BadRequest, "Invalid Data") },
		{ typeof(ExistsException), (StatusCodes.Status400BadRequest, "Bad Request") },
		{ typeof(BCrypt.Net.SaltParseException), (StatusCodes.Status400BadRequest, "Invalid Salt Version") },
	};

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var (statusCode, title) = ExceptionMappings.TryGetValue(exception.GetType(), out var mapping)
			? mapping
			: (StatusCodes.Status500InternalServerError, "Internal Server Error");

		var problemDetails = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = $"{exception.Message}"
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		httpContext.Response.ContentType = "application/json";

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
}
