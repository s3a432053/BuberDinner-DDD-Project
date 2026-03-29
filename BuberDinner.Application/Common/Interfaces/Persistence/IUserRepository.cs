using BuberDinner.Domain.UserAggregate;

namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        public User? GetUserByEmail(string email);
        public void Add(User user);
    }
}
