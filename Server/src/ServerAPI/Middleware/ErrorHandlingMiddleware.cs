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
			catch(NotAcceptableException notAcceptableException)
			{
				//An example of custom Exception Handler in use
				context.Response.StatusCode = 404;
				await context.Response.WriteAsync(notAcceptableException.Message);
			}
			catch(Exception e)
			{
				_logger.LogError(e, e.Message);

				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("An exception has happened on the server.");
			}
		}
	}
}
