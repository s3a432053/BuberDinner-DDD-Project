using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using OneOf;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
                                     IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public AuthenticationResult Login(string email, string password)
        {
            // 驗證 User 是否存在
            if (_userRepository.GetUserByEmail(email) is not User user)
            {
                throw new Exception("User with given email does not exist.");
            }

            // 驗證 Password 是否正確
            if (user.Password != password)
            {
                throw new Exception("Invalid password.");
            }

            // 產出 JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult
            (
                user,
                token
            );
        }

        public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password)
        {
            // 檢查 User 是否已存在
            if (_userRepository.GetUserByEmail(email) is not null)
            {
                return new DuplicateEmailError();
            }

            // 建立 User (產生 Unique ID)
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            _userRepository.Add(user);

            // 建立 JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult
            (
                user,
                token
            );
        }
    }
}
