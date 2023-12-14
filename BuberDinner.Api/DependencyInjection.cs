using BuberDinner.Api.Common.Mapping;
using BuberDinner.Api.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // 加入 Filter
            //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
            services.AddControllers();

            // 改注入 BuberDinnerProblemDetailsFactory 進 ProblemDetailsFactory
            services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();

            // 呼叫剛才建立的 AddMappings 方法做 Mapper 注入
            services.AddMappings();

            return services;
        }
    }
}
