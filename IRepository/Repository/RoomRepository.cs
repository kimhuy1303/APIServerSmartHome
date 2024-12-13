using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository.Repository
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(SmartHomeDbContext context) : base(context)
        {
        }
    }
}
