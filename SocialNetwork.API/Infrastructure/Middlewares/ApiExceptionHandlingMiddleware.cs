﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Domain.Exceptions;
using SocialNetwork.Domain.Shared.ActionResult;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocialNetwork.API.Infrastructure.Middlewares
{
    public class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

        public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsyncV2(context, ex);
                //await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsyncV2(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = Result<string>.Failure(ex.Message);
            switch (ex)
            {
                case DomainException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result;

            if (ex is DomainException)
            {
                var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]> { { "Error", new[] { ex.Message } } })
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(problemDetails);
            }
            else
            {
                _logger.LogError(ex, $"An unhandled exception has occurred, {ex.Message}");
                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error.",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = context.Request.Path,
                    Detail = "Internal server error occurred!"
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(problemDetails);
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}