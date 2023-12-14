using BuberDinner.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuberDinner.Application;

// �̿�`�J�A�� => ���A�ȷ|�b API �M�� �� Program ���[�J
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // �N Mediator �`�J
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // �`�J Behavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // �[�J Validator => �� Function �|�ۦ� �z�� Validator �����{�����`�J
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}