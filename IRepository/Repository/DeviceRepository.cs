using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository.Repository
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(SmartHomeDbContext context) : base(context)
        {
        }
    }
}
