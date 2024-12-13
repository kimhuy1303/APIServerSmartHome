using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository.Repository
{
    public class RFIDCardRepository : RepositoryBase<RFIDCard>, IRFIDCardRepository
    {
        public RFIDCardRepository(SmartHomeDbContext context) : base(context)
        {
        }
    }
}
