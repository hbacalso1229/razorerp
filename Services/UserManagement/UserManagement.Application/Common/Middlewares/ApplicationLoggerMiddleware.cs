using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Text;

namespace UserManagement.Application.Common.Middlewares
{
    public class ApplicationLoggerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("UserName", context.User.Identity.Name);

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning
            string requestBody = "";
            context.Request.EnableBuffering();
            Stream body = context.Request.Body;
            byte[] buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            context.Request.Body = body;

            Log.ForContext("RequestHeaders", context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
               .ForContext("RequestBody", requestBody)
               .Debug("Request information {RequestMethod} {RequestPath} information", context.Request.Method, context.Request.Path);

            Log.Information(string.Format("Request Body: {0} ", requestBody));
            // The reponse body is also a stream so we need to:
            // - hold a reference to the original response body stream
            // - re-point the response body to a new memory stream
            // - read the response body after the request is handled into our memory stream
            // - copy the response in the memory stream out to the original response stream
            using (var responseBodyMemoryStream = new MemoryStream())
            {
                var originalResponseBodyReference = context.Response.Body;
                context.Response.Body = responseBodyMemoryStream;

                await next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                Log.ForContext("RequestBody", requestBody)
                   .ForContext("ResponseBody", responseBody)
                   .Debug("Response information {RequestMethod} {RequestPath} {statusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);

                await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
            }
        }
    }
}
