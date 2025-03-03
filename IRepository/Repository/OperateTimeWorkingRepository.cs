using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIServerSmartHome.IRepository.Repository
{
    public class OperateTimeWorkingRepository : IOperateTimeWorkingRepository
    {
        private readonly SmartHomeDbContext _dbContext;
        public OperateTimeWorkingRepository(SmartHomeDbContext dbContext) 
        {
            _dbContext = dbContext;       
        }
        public async Task Add(OperateTimeWorking request, int deviceId)
        {
            request.DeviceId = deviceId;
            await _dbContext.OperateTimeWorkings.AddAsync(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<List<OperateTimeWorking>>> GetAll(int userId)
        {
            var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId).Select(ud => ud.Device).ToListAsync();
            ICollection<List<OperateTimeWorking>> operateTimeWorkings = new HashSet<List<OperateTimeWorking>>();
            foreach (var userDevice in userDevices) {
                var ope = _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == userDevice.Id).ToList();
                operateTimeWorkings.Add(ope);
            }
            return operateTimeWorkings;
        }

        public async Task<ICollection<OperateTimeWorking>> GetAllByDeviceId(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).ToListAsync();
            var res = await _dbContext.OperateTimeWorkings.Where(otw => otw.DeviceId == deviceId).ToListAsync();
            return res;
        }

        public async Task<ICollection<OperateTimeWorking>> GetHistoryActiveByDeviceId(int deviceId, int userId)
        {
            var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.DeviceId).ToListAsync();
            var res = await _dbContext.OperateTimeWorkings.Where(otw => userDevices.Contains(otw.DeviceId)).Include(otw => otw.Device).Where(e => e.State == Enum.State.ON).ToListAsync();
            return res.OrderByDescending(e => e.OperatingTime).ToList();
        }

        public async Task<int> TimeDeviceWorkingTotalInDay(int deviceId, int userId, DateTime date)
        {
            var userDevice = await _dbContext.UserDevices
                                    .Where(ud => ud.UserId == userId && ud.DeviceId == deviceId)
                                    .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                    .Where(otw => otw.OperatingTime.HasValue && otw.OperatingTime.Value.Date == date.Date)
                                    .OrderBy(otw => otw.OperatingTime)
                                    .ToListAsync();
            // Tính tổng thời gian hoạt động
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach(var record in userDevice)
            {
                if(record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }else if(record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null ;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;
        }

        public async Task<int> TimeDeviceWorkingTotalInMonth(int deviceId, int userId, int month, int year)
        {
            var userDevice = await _dbContext.UserDevices
                                    .Where(ud => ud.UserId == userId && ud.DeviceId == deviceId)
                                    .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                    .Where(otw => otw.OperatingTime.HasValue && otw.OperatingTime.Value.Month == month && otw.OperatingTime.Value.Year == year)
                                    .OrderBy(otw => otw.OperatingTime)
                                    .ToListAsync();
            // Tính tổng thời gian hoạt động
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach (var record in userDevice)
            {
                if (record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }
                else if (record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;
        }

        public async Task<int> TimeDeviceWorkingTotalInWeek(int deviceId, int userId, int week, int year)
        {
            var userDevice = await _dbContext.UserDevices
                                   .Where(ud => ud.UserId == userId && ud.DeviceId == deviceId)
                                   .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                   .Where(otw => otw.OperatingTime.HasValue && ISOWeek.GetWeekOfYear(otw.OperatingTime.Value) == week && otw.OperatingTime.Value.Year == year)
                                   .OrderBy(otw => otw.OperatingTime)
                                   .ToListAsync();
            // Lọc theo tuần (dựa trên tuần của năm)
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach (var record in userDevice)
            {
                if (record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }
                else if (record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;
        }

        public async Task<int> TimeWorkingTotalInDay(int userId, DateTime date)
        {
            var userDevice = await _dbContext.UserDevices
                                    .Where(ud => ud.UserId == userId)
                                    .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                    .Where(otw => otw.OperatingTime.HasValue && otw.OperatingTime.Value.Date == date.Date)
                                    .OrderBy(otw => otw.OperatingTime)
                                    .ToListAsync();
            // Tính tổng thời gian hoạt động
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach (var record in userDevice)
            {
                if (record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }
                else if (record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;

        }

        public async Task<int> TimeWorkingTotalInMonth(int userId, int month, int year)
        {
            var userDevice = await _dbContext.UserDevices
                                    .Where(ud => ud.UserId == userId)
                                    .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                    .Where(otw => otw.OperatingTime.HasValue && otw.OperatingTime.Value.Month == month && otw.OperatingTime.Value.Year == year)
                                    .OrderBy(otw => otw.OperatingTime)
                                    .ToListAsync();
            // Tính tổng thời gian hoạt động
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach (var record in userDevice)
            {
                if (record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }
                else if (record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;
        }

        public async Task<int> TimeWorkingTotalInWeek(int userId, int week, int year)
        {
            var userDevice = await _dbContext.UserDevices
                                    .Where(ud => ud.UserId == userId)
                                    .SelectMany(ud => ud.Device!.OperateTimeWorkings)
                                    .Where(otw => otw.OperatingTime.HasValue && ISOWeek.GetWeekOfYear(otw.OperatingTime.Value) == week && otw.OperatingTime.Value.Year == year)
                                    .OrderBy(otw => otw.OperatingTime)
                                    .ToListAsync();
            // Lọc theo tuần (dựa trên tuần của năm)
            TimeSpan totalOperatingTime = TimeSpan.Zero;
            DateTime? LastStartTime = null;

            foreach (var record in userDevice)
            {
                if (record.State == Enum.State.ON)
                {
                    LastStartTime = record.OperatingTime!.Value;
                }
                else if (record.State == Enum.State.OFF && LastStartTime.HasValue)
                {
                    totalOperatingTime += record.OperatingTime!.Value - LastStartTime.Value;
                    LastStartTime = null;
                }
            }
            return (int)totalOperatingTime.TotalSeconds;
        }
    }
}
