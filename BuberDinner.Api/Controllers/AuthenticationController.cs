using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("Auth")]
// AllowAnonymous Attribute 可以允許任何 Request 進來此 Controller 不受 Authorization 的驗證影響
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        // 改用 Result<AuthenticationResult> 型別去接資料
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        // 透過 Match 判斷是要回傳 AuthenticationResult Or List<Error>
        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
            );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

        //if (authResult.IsError && authResult.FirstError == BuberDinner.Domain.Common.Errors.Errors.Authentication.InvalidCredentials)
        //{
        //    return Problem(
        //        statusCode: StatusCodes.Status401Unauthorized,
        //        title: authResult.FirstError.Description);
        //}

        // 透過 Match 判斷是要回傳 AuthenticationResult Or List<Error>
        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
            );
    }
}