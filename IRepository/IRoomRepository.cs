using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        Task AddDeviceIntoSite(Device device, int RoomId);
        Task RemoveDeviceFromSite(int RoomId, int DeviceId);
        Task<List<Device>> GetAllDevicesInSite(int RoomId);
    }
}
