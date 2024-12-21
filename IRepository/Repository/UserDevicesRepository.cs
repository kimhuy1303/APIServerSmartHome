using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository.Repository
{
    public class UserDevicesRepository : IUserDevicesRepository
    {
        private readonly SmartHomeDbContext _dbContext;
        public UserDevicesRepository(SmartHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(UserDevices request)
        {
            await _dbContext.UserDevices.AddAsync(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
