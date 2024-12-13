using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IPowerDeviceRepository 
    {
        Task AddAsync(PowerDevice powerDevice);
        Task<IEnumerable<PowerDevice>> GetAllAsync();
    }
}
