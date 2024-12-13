using APIServerSmartHome.Data;
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
        public async Task AddAsync(PowerDevice entity)
        {
            await _context.PowerDevices.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PowerDevice>> GetAllAsync()
        {
            return await _context.PowerDevices.ToListAsync();
        }

        
    }
}
