using APIServerSmartHome.IRepository;

namespace APIServerSmartHome.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IDeviceRepository Devices { get; }
        IPowerDeviceRepository PowerDevices { get; }
        IRFIDCardRepository Cards { get; }
        IRoomRepository Rooms { get; }
        IUserRepository Users { get; }
        IUserFacesRepository UserFaces { get; }
        IOperateTimeWorkingRepository OperateTimeWorkings { get; }
        IUserDevicesRepository UserDevices { get; }
        IIrrigationScheduleRepository IrrigationSchedules { get; }
    }
}
