using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IPowerDeviceRepository 
    {
        Task AddAsync(PowerDeviceDTO request);
        Task<IEnumerable<PowerDevice>> GetAllAsync(int userId);
        Task<PowerDevice> GetByDeviceIdAsync(int deviceId, int userId);
       
    }
}
