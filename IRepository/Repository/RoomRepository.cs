using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIServerSmartHome.IRepository.Repository
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(SmartHomeDbContext context) : base(context)
        {
        }

        public async Task AddDeviceIntoSite(Device device, int RoomId)
        {
            _dbContext.Attach(device);
            device.RoomId = RoomId;
            _dbContext.Devices.Update(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Device>> GetAllDevicesInSite(int RoomId)
        {
            return await _dbContext.Devices.Where(device => device.RoomId == RoomId).ToListAsync();
        }

        public async Task RemoveDeviceFromSite(int RoomId, int DeviceId)
        {
            var devices = await GetAllDevicesInSite(RoomId);
            var deviceToRemove = devices.FirstOrDefault(d => d.Id == DeviceId);
            deviceToRemove.RoomId = null;
            _dbContext.Devices.Update(deviceToRemove);
            await _dbContext.SaveChangesAsync();
        }
    }
}
