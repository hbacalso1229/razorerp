using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Dynamic;
using System.Reflection;
using ApplicationException = UserManagement.Application.Common.Exceptions.Base.ApplicationException;
using UserManagement.Application.Common.Exceptions.Base;

namespace UserManagement.Application.Common.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
        {
            dynamic response = new ExpandoObject();

            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Internal Server Error";

            //TODO
            GetAggregateExceptionStatusCode(exception, exception as AggregateException, out statusCode, out title);

            var exceptionMessage = new
            {
                Application = Assembly.GetExecutingAssembly().GetName().Name,
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Title = title,
                Status = statusCode,
                exception.Source,
                exception.Message,
                Details = GetErrors(exception)
            };

            response.Data = new { };
            response.Success = statusCode.ToString().StartsWith(4.ToString()) ? true : false;
            response.DateTime = DateTime.Now.ToString();

            if (response.Success)
            {
                response.Warning = exceptionMessage;
                logger.LogInformation(exceptionMessage.Title, new { response });
            }
            else
            {
                response.Error = exceptionMessage;
                logger.LogError(exceptionMessage.Title, new { response });
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            string content = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });


            await httpContext.Response.WriteAsync(content);
        }

        private static int GetStatusCode(Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            //TODO
            Type type = exception.GetType().Name.Equals(nameof(ValidationException)) ? exception.GetType() : exception.GetType().BaseType;

            switch (type.Name)
            {
                case nameof(ValidationException):
                case nameof(BadRequestException):
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case nameof(NotFoundException):
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case nameof(UnprocessableEntityException):
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    break;
                case nameof(ServiceUnavailableException):
                    statusCode = StatusCodes.Status503ServiceUnavailable;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            return statusCode;
        }

        private static string GetTitle(Exception exception) =>
            exception switch
            {
                ApplicationException applicationException => applicationException.Title,
                _ => "Internal Server Error"
            };

        private static object GetErrors(Exception exception)
        {
            dynamic errors = new ExpandoObject();

            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors;
            }
            else
            {
                if (exception is AggregateException aggregateException)
                {
                    //TODO
                    if (exception != null)
                    {
                        var data = aggregateException.Flatten().InnerException?.Data ?? aggregateException.Data;
                        string message = aggregateException.Flatten().InnerException?.Message ?? aggregateException.Message;

                        errors = aggregateException.Data.Count > 0 ?
                            new { aggregateException.Data, aggregateException.Message } :
                            new { data, message };
                    }
                }
                else
                {
                    var data = exception.InnerException?.Data ?? exception.Data;
                    string message = exception.InnerException?.Message ?? exception.Message;

                    errors = exception.Data.Count > 0 ?
                        new { exception.Data, exception.Message } :
                        new { data, message };
                }
            }

            return errors;
        }

        private static bool IsCancellationException(Exception ex)
        {
            if (ex == null) return false;
            if (ex is OperationCanceledException)
                return true;
            if (ex is IOException && ex.Message == "The client reset the request stream.")
                return true;
            if (ex is Microsoft.AspNetCore.Connections.ConnectionResetException)
                return true;

            return false;
        }

        //TODO
        private static void GetAggregateExceptionStatusCode(Exception exception, AggregateException aggregateException, out int statusCode, out string title)
        {
            statusCode = StatusCodes.Status500InternalServerError;
            title = "Internal Server Error";

            if (aggregateException != null)
            {
                foreach (dynamic ex in aggregateException.InnerExceptions)
                {
                    switch (ex)
                    {
                        case ValidationException:
                        case BadRequestException:
                            statusCode = StatusCodes.Status400BadRequest;
                            title = ex.Title;
                            break;
                        case NotFoundException:
                            statusCode = StatusCodes.Status404NotFound;
                            title = ex.Title;
                            break;
                        case UnprocessableEntityException:
                            statusCode = StatusCodes.Status422UnprocessableEntity;
                            break;
                        case ServiceUnavailableException:
                            statusCode = StatusCodes.Status503ServiceUnavailable;
                            title = ex.Title;
                            break;
                        default:
                            statusCode = StatusCodes.Status500InternalServerError;
                            title = ex.Title;
                            break;
                    }
                }
            }
            else
            {
                statusCode = GetStatusCode(exception);
                title = GetTitle(exception);
            }
        }
    }
}
