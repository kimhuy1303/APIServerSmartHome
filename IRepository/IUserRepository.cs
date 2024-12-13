using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
        bool VerifyPassword(User user, string password);
    }
}
