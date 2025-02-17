using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServerSmartHome.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    [Authorize]
    public class PowerDeviceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PowerDeviceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllPowers()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var res = await _unitOfWork.PowerDevices.GetAllAsync(userId);
            return Ok(res);
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult> GetPowerUsingByDeviceId(int deviceId)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var res = await _unitOfWork.PowerDevices.GetByDeviceIdAsync(deviceId, userId);
            return Ok(res);
        }

        [HttpPost()]
        public async Task<ActionResult> AddPowerDevice(PowerDeviceDTO request)
        {
            await _unitOfWork.PowerDevices.AddAsync(request);
            return Ok(request);
        }


    }
}
