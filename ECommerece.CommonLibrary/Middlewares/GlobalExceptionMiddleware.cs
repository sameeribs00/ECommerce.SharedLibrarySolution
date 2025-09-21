namespace ECommerece.CommonLibrary.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string title = "Error";
            string message = "Sorry, internal server error occurred. Kindly try again";
            int statusCode = (int)StatusCodes.Status500InternalServerError;

            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Error: Too Many Requests";
                    message = "Too many request, please avoid this behavior next time";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context,title,message,statusCode);
                }
                if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    title = "Error: Bad Request";
                    message = "Please check the sent parameters, url syntax or file size if sent";
                    statusCode = (int)StatusCodes.Status400BadRequest;
                    await ModifyHeader(context,title,message,statusCode);
                }
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Error: Unauthorized";
                    message = "You are not recognized as a user in this system";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context,title,message,statusCode);
                }
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Error: Forbidden";
                    message = "You are not allowed to perform this action";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if(ex is TaskCanceledException or TimeoutException)
                {
                    title = "Error: Time Out";
                    message = "Request time out, please try again";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            var problem = new ProblemDetails
            {
                Title = title,
                Detail = message,
                Status = statusCode,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem), CancellationToken.None);
        }
    }
}