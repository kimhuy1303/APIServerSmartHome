using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IOperateTimeWorkingRepository
    {
        Task Add(OperateTimeWorking request, int deviceId);
        Task<ICollection<List<OperateTimeWorking>>> GetAll(int userId);
        Task<ICollection<OperateTimeWorking>> GetAllByDeviceId(int deviceId, int userId);
        Task<int> TimeDeviceWorkingTotalInDay(int deviceId, int userId, DateTime date);
        Task<int> TimeDeviceWorkingTotalInWeek(int deviceId, int userId, int week, int year);
        Task<int> TimeDeviceWorkingTotalInMonth(int deviceId, int userId, int month, int year);
        Task<int> TimeWorkingTotalInDay(int userId, DateTime date);
        Task<int> TimeWorkingTotalInWeek(int userId, int week, int year);
        Task<int> TimeWorkingTotalInMonth(int userId, int month, int year);

        Task<ICollection<OperateTimeWorking>> GetHistoryActiveByDeviceId(int deviceId, int userId); 
    }
}
