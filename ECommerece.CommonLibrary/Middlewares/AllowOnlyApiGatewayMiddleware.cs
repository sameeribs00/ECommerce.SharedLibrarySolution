namespace ECommerece.CommonLibrary.Middlewares
{
    public class AllowOnlyApiGatewayMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var apiGatewayHeader = context.Request.Headers["Api-Gateway"];
            if(apiGatewayHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
                {
                    Title = "Not api gateway request",
                    Status = StatusCodes.Status503ServiceUnavailable,
                    Detail = "Not allowed request, api gateway GAURD is here"
                }), CancellationToken.None);
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
