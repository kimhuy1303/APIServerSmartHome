using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;

namespace APIServerSmartHome.IRepository
{
    public interface IDeviceRepository : IRepositoryBase<Device>
    {
        Task ChangeStateDevice(Device device, State newState);
        Task<ICollection<OperateTimeWorking>> GetStatesDevice(int deviceId, int userId);
        Task<List<OperateTimeWorking>> getStatesAllDevices(int userId);

        Task<List<Device>> getActiveAllDevices(int userId);
        Task<List<Device>> getActiveDevicesBySite(int userId, int siteId);
    }
}
