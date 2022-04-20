namespace backend.Recycle.Extensions.Services
{
    public static class ApplicationCollection
    {
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            return builder;
        }

    }
}
