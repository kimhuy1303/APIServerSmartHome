using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IIrrigationScheduleRepository
    {
        Task AddSchedule(IrrigationSchedule schedule);
        Task HandleActive(IrrigationSchedule schedule);
        Task ChangeTimeWorking(IrrigationSchedule schedule, DateTime timeWorking);
        Task<IrrigationSchedule> GetSchedule();
        Task<IrrigationSchedule> GetScheduleById(int id);
    }
}
