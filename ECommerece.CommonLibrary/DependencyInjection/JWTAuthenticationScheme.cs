namespace ECommerece.CommonLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJwtAuthenticationScheme(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                             .AddJwtBearer("Bearer", options =>
                             {
                                 var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                                 var issuer = config.GetSection("Authentication:Issuer").Value!;
                                 var auddience = config.GetSection("Authentication:Auddience").Value!;

                                 options.RequireHttpsMetadata = false;
                                 options.SaveToken = true;
                                 options.TokenValidationParameters = new TokenValidationParameters
                                 {
                                     ValidateIssuer = true,
                                     ValidateAudience = true,
                                     ValidateLifetime = false, //this because we dont have refresh token for now, make it true if you want to have it 
                                     ValidateIssuerSigningKey = true,

                                     ValidIssuer = issuer,
                                     ValidAudience = auddience,
                                     IssuerSigningKey = new SymmetricSecurityKey(key)
                                 };
                             });

            return serviceCollection;
        }
    }
}
