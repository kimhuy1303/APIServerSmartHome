using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIServerSmartHome.IRepository.Repository
{
    public class IrrigationScheduleRepository : IIrrigationScheduleRepository
    {
        private readonly SmartHomeDbContext _dbContext;
        public IrrigationScheduleRepository(SmartHomeDbContext dbContext) 
        {
            _dbContext = dbContext;        
        }

        public async Task AddSchedule(IrrigationSchedule schedule)
        {
            await _dbContext.AddAsync(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeTimeWorking(IrrigationSchedule schedule, DateTime timeWorking)
        {
            schedule.TimeWorking = timeWorking;
            _dbContext.IrrigationSchedules.Update(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IrrigationSchedule> GetSchedule()
        {
            return await _dbContext.IrrigationSchedules.FirstOrDefaultAsync();
        }

        public async Task<IrrigationSchedule> GetScheduleById(int id)
        {
            return await _dbContext.IrrigationSchedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task HandleActive(IrrigationSchedule schedule)
        {
            schedule.IsActive = schedule.IsActive == true ? false : true;
            _dbContext.IrrigationSchedules.Update(schedule);
            await _dbContext.SaveChangesAsync();
        }
    }
}
