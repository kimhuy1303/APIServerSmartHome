using APIServerSmartHome.Data;
using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APIServerSmartHome.IRepository.Repository
{
    public class PowerDeviceRepository : IPowerDeviceRepository
    {
        private readonly SmartHomeDbContext _context;
        public PowerDeviceRepository(SmartHomeDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PowerDeviceDTO dto)
        {
            var powerDevice = new PowerDevice
            {
                DeviceId = dto.DeviceId,
                TimeUsing = dto.TimeUsing,
                PowerValue = dto.PowerValue
            };
            await _context.PowerDevices.AddAsync(powerDevice);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PowerDevice>> GetAllAsync(int userId)
        {
            var userDevices = await _context.UserDevices.Where(x => x.UserId == userId).Select(x => x.Device).ToListAsync();
            var powerDevices = new List<PowerDevice>();
            foreach (var userDevice in userDevices)
            {
                var powerDevice = await _context.PowerDevices.FirstOrDefaultAsync(x => x.DeviceId == userDevice!.Id);
                powerDevices.Add(powerDevice!);
            }
            return powerDevices;
        }

        public async Task<IEnumerable<PowerDevice>> GetAllByDeviceIdAsync(int deviceId)
        {
            var powerDevices = await _context.PowerDevices.Where(x => x.DeviceId == deviceId).OrderBy(x => x.TimeUsing).ToListAsync();
            return powerDevices;
        }

        public async Task<PowerDevice> GetByDeviceIdAsync(int deviceId, int userId)
        {
            var userDevice = await _context.UserDevices.FirstOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId);
            var powerDevice = await _context.PowerDevices.FirstOrDefaultAsync(x => x.DeviceId == userDevice.DeviceId);
            return powerDevice!;
        }

        public async Task<double> GetTotalPowerUsingAsync(int userId)
        {
            var userDevices = await _context.UserDevices.Where(x => x.UserId == userId).Select(x => x.Device).ToListAsync();
            var powerDevices = 0.0;
            foreach (var userDevice in userDevices)
            {
                var powerDevice = await GetTotalPowerUsingByDeviceIdAsync(userDevice!.Id);
                powerDevices += powerDevice;
            }

            return Math.Round(powerDevices,4);
        }

        public async Task<double> GetTotalPowerUsingByDeviceIdAsync(int deviceId)
        {
            var powerDevices = await _context.PowerDevices.Where(x => x.DeviceId == deviceId).SumAsync(e => e.PowerValue);
            return Math.Round(powerDevices,4);
        }
    }
}
