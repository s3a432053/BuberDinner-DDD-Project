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
    // 加入 Middleware
    // app.UseMiddleware<ErrorHandlingMiddleware>();

    // 捕捉到 Exception 導向 ErrorController
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();

    // 驗證 Client 身分，判斷是否授權
    app.UseAuthentication();

    // 判斷 哪些 API 是 已授權 Client 可以存取的
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}