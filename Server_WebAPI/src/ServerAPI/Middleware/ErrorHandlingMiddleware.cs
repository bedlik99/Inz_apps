using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServerAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Middleware
{
	public class ErrorHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
		{
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (BadRequestException badRequestException)
			{
				context.Response.StatusCode = 400;
				await context.Response.WriteAsync(badRequestException.Message);
			}
			catch (NotFoundException notFoundException)
			{
				context.Response.StatusCode = 404;
				await context.Response.WriteAsync(notFoundException.Message);
			}
			catch (NotAuthorizedException notAuthorizedException)
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync(notAuthorizedException.Message);
			}
			catch (NotAcceptableException notAcceptableException)
			{
				context.Response.StatusCode = 404;
				await context.Response.WriteAsync(notAcceptableException.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("An exception has occured on the server.");
			}
		}
	}
}

