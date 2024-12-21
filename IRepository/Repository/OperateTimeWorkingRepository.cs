using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> TimeDeviceWorkingTotalInDay(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<int> TimeDeviceWorkingTotalInMonth(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<int> TimeDeviceWorkingTotalInWeek(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).ToListAsync();
            throw new NotImplementedException();
        }

        public Task<int> TimeWorkingTotalInDay(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> TimeWorkingTotalInMonth(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> TimeWorkingTotalInWeek(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
