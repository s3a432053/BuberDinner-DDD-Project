using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public AuthenticationResult Login(string email, string password)
        {
            return new AuthenticationResult
            (
                Guid.NewGuid(),
                "firstName",
                "lastName",
                email,
                "token"
            );
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            // 檢查 User 是否已存在

            // 建立 User (產生 Unique ID)

            // 建立 JWT Token
            Guid userId = Guid.NewGuid();
            var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);

            return new AuthenticationResult
            (
                Guid.NewGuid(),
                firstName,
                lastName,
                email,
                token
            );
        }
    }
}
