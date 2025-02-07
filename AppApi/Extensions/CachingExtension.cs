using App.Application.Contracts.Caching;
using App.Caching;

namespace AppApi.Extensions
{
    public static class CachingExtension
    {
        public static IServiceCollection AddCachingExt(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>(); //veritabanına sorgula gitmeyeceği için singleton olarak ekliyoruz.
            return services;
        }
    }
}
