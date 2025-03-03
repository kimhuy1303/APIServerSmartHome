using APIServerSmartHome.Data;
using APIServerSmartHome.Entities;
using APIServerSmartHome.IRepository;
using APIServerSmartHome.IRepository.Repository;

namespace APIServerSmartHome.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartHomeDbContext _context;
        public IDeviceRepository Devices { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IPowerDeviceRepository PowerDevices { get; private set; }
        public IRFIDCardRepository Cards { get; private set; }
        public IUserFacesRepository UserFaces { get; private set; }
        public IUserRepository Users{ get; private set; }

        public IOperateTimeWorkingRepository OperateTimeWorkings {  get; private set; }
        public IUserDevicesRepository UserDevices { get; private set; }
        public IIrrigationScheduleRepository IrrigationSchedules { get; private set; }
        
     
        public UnitOfWork(SmartHomeDbContext context) 
        { 
            _context = context;
            Devices = new DeviceRepository(_context);
            Rooms = new RoomRepository(_context);
            PowerDevices = new PowerDeviceRepository(_context);
            Cards = new RFIDCardRepository(_context);
            UserFaces = new UserFacesRepository(_context);
            Users = new UserRepository(_context);
            OperateTimeWorkings = new OperateTimeWorkingRepository(_context);
            UserDevices = new UserDevicesRepository(_context);
            IrrigationSchedules = new IrrigationScheduleRepository(_context);

        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
