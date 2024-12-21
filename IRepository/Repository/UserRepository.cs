using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIServerSmartHome.IRepository.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(SmartHomeDbContext context) : base(context) { }

        public async Task AddDeviceToUser(UserDevices request)
        {
            var userDevice = new UserDevices
            {
                UserId = request.UserId,
                DeviceId = request.DeviceId,
            };
            await _dbContext.UserDevices.AddAsync(userDevice);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPasswordToDevice(int deviceId, int userId, string password)
        {
            var device = await _dbContext.UserDevices.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DeviceId == deviceId);
            if(device!.Password != null)
            {
                device!.Password = password;
                _dbContext.UserDevices.Update(device);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangePasswordFromDevice(int deviceId, int userId, string newPassword)
        {
            var device = await _dbContext.UserDevices.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DeviceId == deviceId);
            device!.Password = newPassword;
            _dbContext.UserDevices.Update(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Device>> GetAllDevices(int userId)
        {
            var userDevices = await _dbContext.UserDevices.Where(ud => ud.UserId == userId).Select(ud => ud.Device).ToListAsync();
            return userDevices;
        }

        public async Task<Device> GetDevice(int userId, int deviceId)
        {
            var userDevice = await _dbContext.UserDevices.Where(ud => ud.UserId == userId && ud.DeviceId == deviceId).Select(ud => ud.Device).FirstOrDefaultAsync();
            return userDevice;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task RemoveDevice(int deviceId, int userId)
        {
            var userDevice = await _dbContext.UserDevices.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DeviceId == deviceId);
            if (userDevice != null)
            {
                _dbContext.UserDevices.Remove(userDevice);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task RemovePasswordFromDevice(int deviceId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }


    }
}
