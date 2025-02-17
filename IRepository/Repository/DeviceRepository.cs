using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace APIServerSmartHome.IRepository.Repository
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(SmartHomeDbContext context) : base(context)
        {
        }

        public async Task ChangeStateDevice(Device device, State state)
        {
            device.State = state;
            _dbContext.Devices.Update(device);
            await _dbContext.SaveChangesAsync();
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            var operateTimeWorking = new OperateTimeWorking
            {
                State = state,
                OperatingTime = vietnamTime,
                DeviceId = device.Id,
            };
            await _dbContext.OperateTimeWorkings.AddAsync(operateTimeWorking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Device>> getActiveAllDevices(int userId)
        {
            var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId).Select(ud => ud.Device).ToListAsync();
            return userDevices.Where(e => e.State == State.ON).ToList();

        }

        public async Task<List<Device>> getActiveDevicesBySite(int userId, int siteId)
        {
            var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId).Select(ud => ud.Device).ToListAsync();
            return userDevices.Where(e => e.RoomId == siteId && e.State == State.ON).ToList();
        }

        public async Task<List<OperateTimeWorking>> getStatesAllDevices(int userId)
        {
           var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId).Select(ud => ud.DeviceId).ToListAsync();
           var res = await _dbContext.OperateTimeWorkings.Where(otw => userDevices.Contains(otw.DeviceId)).Include(otw => otw.Device).ToListAsync();
            return res;
        }

        public async Task<ICollection<OperateTimeWorking>> GetStatesDevice(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).FirstOrDefaultAsync();
            var deviceStates = await _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == userDevice.Id).ToListAsync();
            return deviceStates;
        }
    }
}
