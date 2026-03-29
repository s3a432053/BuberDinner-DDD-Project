using BuberDinner.Domain.UserAggregate;

namespace BuberDinner.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(User user);
    }
}