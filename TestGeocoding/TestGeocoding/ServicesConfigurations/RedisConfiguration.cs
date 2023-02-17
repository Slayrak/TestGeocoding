namespace TestGeocoding.ServicesConfigurations
{
    public static class RedisConfiguration
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "redisCache";
            });

            return services;
        }

    }
}
