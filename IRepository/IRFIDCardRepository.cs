using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;

namespace APIServerSmartHome.IRepository
{
    public interface IRFIDCardRepository : IRepositoryBase<RFIDCard>
    {
        Task<List<RFIDCard>> GetCardsByUser(int userId);
        Task<RFIDCard> GetCardByUser(int cardId, int userId);
        Task<RFIDCard> GetCardByUser(string cardUid, int userId);
        Task ChangeActiveState(RFIDCard card, bool active);
        Task GrantAccessLevel(RFIDCard card, AccessLevel level);
    }
}
