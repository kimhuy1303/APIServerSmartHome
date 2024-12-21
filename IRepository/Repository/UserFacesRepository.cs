using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIServerSmartHome.IRepository.Repository
{
    public class UserFacesRepository : RepositoryBase<UserFaces>, IUserFacesRepository
    {
        public UserFacesRepository(SmartHomeDbContext context) : base(context)
        {
        }

        public async Task<UserFaces> GetFaceDataOfUser(int userFaceId, int userId)
        {
            return await _dbContext.UserFaces.FirstOrDefaultAsync(uf => uf.UserId == userId && uf.Id == userFaceId);
        }
    }
}
