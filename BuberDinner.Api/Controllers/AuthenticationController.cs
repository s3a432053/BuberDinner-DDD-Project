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
// AllowAnonymous Attribute �i�H���\���� Request �i�Ӧ� Controller ���� Authorization �����Ҽv�T
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

        // ��� Result<AuthenticationResult> ���O�h�����
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        // �z�L Match �P�_�O�n�^�� AuthenticationResult Or List<Error>
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

        // �z�L Match �P�_�O�n�^�� AuthenticationResult Or List<Error>
        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
            );
    }
}