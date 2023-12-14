using BuberDinner.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuberDinner.Application;

// 依賴注入服務 => 此服務會在 API 專案 的 Program 中加入
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 將 Mediator 注入
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // 注入 Behavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 加入 Validator => 此 Function 會自行 篩選 Validator 相關程式做注入
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}