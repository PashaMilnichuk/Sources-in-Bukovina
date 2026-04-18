using System.Net;
using System.Text.Json;

namespace CarpathianCrown.Api.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
	private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			context.Response.ContentType = "application/json";

			var (status, title) = ex switch
			{
				UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
				KeyNotFoundException => (HttpStatusCode.NotFound, "Not found"),
				ArgumentException => (HttpStatusCode.BadRequest, "Bad request"),
				InvalidOperationException => (HttpStatusCode.Conflict, "Conflict"),
				_ => (HttpStatusCode.InternalServerError, "Server error")
			};

			context.Response.StatusCode = (int)status;

			var body = new
			{
				error = title,
				detail = ex.Message,
				traceId = context.TraceIdentifier
			};

			await context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOpts));
		}
	}
}