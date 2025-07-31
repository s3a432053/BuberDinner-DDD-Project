using BuberDinner.Application.Common.Errors;
using OneOf;

namespace BuberDinner.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password);
        public AuthenticationResult Login(string email, string password);
    }
}
