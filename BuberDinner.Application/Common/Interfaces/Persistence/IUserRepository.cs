using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        public User? GetUserByEmail(string email);
        public void Add(User user);
    }
}
