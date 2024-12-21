using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
        bool VerifyPassword(User user, string password);
        Task<List<Device>> GetAllDevices(int userId);
        Task<Device> GetDevice(int userId,int deviceId);
        Task AddDeviceToUser(UserDevices request);
        Task RemoveDevice(int deviceId, int userId);
        Task AddPasswordToDevice(int deviceId, int userId, string password);
        Task RemovePasswordFromDevice(int deviceId, int userId);
        Task ChangePasswordFromDevice(int deviceId, int userId, string newPassword);

    }
}
