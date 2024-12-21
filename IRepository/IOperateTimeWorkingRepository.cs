using APIServerSmartHome.Entities;

namespace APIServerSmartHome.IRepository
{
    public interface IOperateTimeWorkingRepository
    {
        Task Add(OperateTimeWorking request, int deviceId);
        Task<ICollection<List<OperateTimeWorking>>> GetAll(int userId);
        Task<ICollection<OperateTimeWorking>> GetAllByDeviceId(int deviceId, int userId);
        Task<int> TimeDeviceWorkingTotalInDay(int deviceId, int userId);
        Task<int> TimeDeviceWorkingTotalInWeek(int deviceId, int userId);
        Task<int> TimeDeviceWorkingTotalInMonth(int deviceId, int userId);
        Task<int> TimeWorkingTotalInDay(int userId);
        Task<int> TimeWorkingTotalInWeek(int userId);
        Task<int> TimeWorkingTotalInMonth(int userId);
    }
}
