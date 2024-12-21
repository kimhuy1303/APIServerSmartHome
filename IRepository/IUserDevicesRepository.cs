using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IUserDevicesRepository 
    {
        Task AddAsync(UserDevices request);
    }
}
