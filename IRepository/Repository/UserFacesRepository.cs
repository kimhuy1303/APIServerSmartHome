using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository.Repository
{
    public class UserFacesRepository : RepositoryBase<UserFaces>, IUserFacesRepository
    {
        public UserFacesRepository(SmartHomeDbContext context) : base(context)
        {
        }
    }
}
