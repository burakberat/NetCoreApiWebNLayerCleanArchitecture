using AppApi.Filters;

namespace AppApi.Extensions
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddControllerWithFiltersExt(this IServiceCollection services)
        {
            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler için null kontrolünü kendisi yapmayacak.
            });
            return services;
        }
    }
}
