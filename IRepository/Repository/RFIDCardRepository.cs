using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;
using Microsoft.EntityFrameworkCore;

namespace APIServerSmartHome.IRepository.Repository
{
    public class RFIDCardRepository : RepositoryBase<RFIDCard>, IRFIDCardRepository
    {
        public RFIDCardRepository(SmartHomeDbContext context) : base(context)
        {
        }

        public async Task ChangeActiveState(RFIDCard card, bool active)
        {
            card.IsActive = active;
            _dbContext.RFIDCards.Update(card);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RFIDCard> GetCardByUser(int cardId, int userId)
        {
            return await _dbContext.RFIDCards.FirstOrDefaultAsync(c => c.Id == cardId && c.UserId == userId);
        }

        public async Task<RFIDCard> GetCardByUser(string cardUid, int userId)
        {
            return await _dbContext.RFIDCards.FirstOrDefaultAsync(c => c.CardUID == cardUid && c.UserId == userId);
        }

        public async Task<List<RFIDCard>> GetCardsByUser(int userId)
        {
            return await _dbContext.RFIDCards.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task GrantAccessLevel(RFIDCard card, AccessLevel level)
        {
            card.AccessLevel = level;
            _dbContext.RFIDCards.Update(card);
            await _dbContext.SaveChangesAsync();
        }
    }
}
