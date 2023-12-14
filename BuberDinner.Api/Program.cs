using BuberDinner.Api;
using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    // �[�J Middleware
    // app.UseMiddleware<ErrorHandlingMiddleware>();

    // ������ Exception �ɦV ErrorController
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();

    // ���� Client �����A�P�_�O�_���v
    app.UseAuthentication();

    // �P�_ ���� API �O �w���v Client �i�H�s����
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}