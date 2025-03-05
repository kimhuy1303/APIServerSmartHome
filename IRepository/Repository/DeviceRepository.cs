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

        public async Task ChangeStateAllFans(State newState)
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            var fans = await _dbContext.Devices.Where(d => d.DeviceName!.StartsWith("Quạt") || d.DeviceName!.StartsWith("Máy lạnh")).ToListAsync();
            foreach (var fan in fans)
            {
                if(fan.State == newState) continue;
                var lastOperate = await _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == fan.Id).OrderByDescending(otw => otw.OperatingTime).FirstOrDefaultAsync();
                if (lastOperate?.OperatingTime != null && lastOperate.State == State.ON && newState == State.OFF)
                {
                    var duration = vietnamTime - lastOperate.OperatingTime!.Value;
                    var devicePower = new PowerDevice
                    {
                        PowerValue = Math.Round((Math.Round(duration.TotalHours, 2) * CapacityDevice.MiniFan) / 1000.0, 4),
                        TimeUsing = lastOperate.OperatingTime.Value,
                        DeviceId = fan.Id,
                    };
                    await _dbContext.PowerDevices.AddAsync(devicePower);
                }

                fan.State = newState;
                _dbContext.Devices.Update(fan);
                var operateTimeWorking = new OperateTimeWorking
                {
                    State = newState,
                    OperatingTime = vietnamTime,
                    DeviceId = fan.Id,
                };
                await _dbContext.OperateTimeWorkings.AddAsync(operateTimeWorking);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangeStateAllLights(State newState)
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            var lights = await _dbContext.Devices.Where(d => d.DeviceName!.StartsWith("Đèn")).ToListAsync();
            foreach (var light in lights)
            {
                if(light.State == newState) continue;
                var lastOperate = await _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == light.Id).OrderByDescending(otw => otw.OperatingTime).FirstOrDefaultAsync();
                if (lastOperate?.OperatingTime != null && lastOperate.State == State.ON && newState == State.OFF)
                {
                    var duration = vietnamTime - lastOperate.OperatingTime!.Value;
                    var devicePower = new PowerDevice
                    {
                        PowerValue = Math.Round((Math.Round(duration.TotalHours, 2) * CapacityDevice.Led) / 1000.0, 4),
                        TimeUsing = lastOperate.OperatingTime.Value,
                        DeviceId = light.Id,
                    };
                    await _dbContext.PowerDevices.AddAsync(devicePower);
                }

                light.State = newState;
                _dbContext.Devices.Update(light);
                var operateTimeWorking = new OperateTimeWorking
                {
                    State = newState,
                    OperatingTime = vietnamTime,
                    DeviceId = light.Id,
                };
                await _dbContext.OperateTimeWorkings.AddAsync(operateTimeWorking);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangeStateDevice(Device device, State newState)
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            var capacity = device.DeviceName switch
            {
                var name when name.StartsWith("Cửa") => CapacityDevice.Servo,
                var name when name.StartsWith("Đèn") => CapacityDevice.Led,
                var name when name.StartsWith("Quạt") => CapacityDevice.MiniFan,
                var name when name.StartsWith("Máy") => CapacityDevice.WaterPump,
                _ => 0.0
            };
            var lastOperate = await _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == device.Id).OrderByDescending(otw => otw.OperatingTime).FirstOrDefaultAsync();
            if(lastOperate?.OperatingTime != null && lastOperate.State == State.ON && newState == State.OFF)
            {
                var duration = vietnamTime - lastOperate.OperatingTime!.Value;
                var devicePower = new PowerDevice
                {
                    PowerValue = Math.Round((Math.Round(duration.TotalHours,2) * capacity) / 1000.0,4),
                    TimeUsing = lastOperate.OperatingTime.Value,
                    DeviceId = device.Id,
                };
                await _dbContext.PowerDevices.AddAsync(devicePower);
            }

            device.State = newState;
            _dbContext.Devices.Update(device);
            var operateTimeWorking = new OperateTimeWorking
            {
                State = newState,
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

        public async Task<List<Device>> getAvailableDevices(int userId)
        {
            var availableDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.Device!.RoomId == null).Select(d => d.Device).ToListAsync();   
            return availableDevices;
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
