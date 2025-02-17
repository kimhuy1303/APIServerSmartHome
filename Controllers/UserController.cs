using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;
using APIServerSmartHome.Services;
using APIServerSmartHome.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Security.Claims;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }
        [HttpGet]       
        public async Task<ActionResult> GetUser()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if(string.IsNullOrEmpty(userId)) return Unauthorized(new {message = "User not authenticated!"});
            var user = await _unitOfWork.Users.GetByIdAsync(Int32.Parse(userId));
            return Ok(user);
        }

        [HttpPost("send-email")]
        public async Task<ActionResult> SendEmail([FromBody] EmailMessageDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body))
            {
                return BadRequest("Invalid email message.");
            }
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            return Ok(new { message = "Send email successfully!" });
        }

        [HttpPut("update-infomation")]
        public async Task<ActionResult> UpdateProfile(ProfileDTO profile)
        {
            try
            {
                var id = User.FindFirst("UserId")?.Value!; 
                if(id.IsNullOrEmpty()) return Unauthorized("User not authenticated!");
                if (ModelState.IsValid)
                {
                    var existing_user = await _unitOfWork.Users.GetByIdAsync(Int32.Parse(id));
                    if (existing_user != null)
                    {
                        _mapper.Map(profile, existing_user);
                    }
                    await _unitOfWork.Users.UpdateAsync(Int32.Parse(id), existing_user!);
                    return Ok(new {message = "Update infomation successfully!", user = existing_user });
                }
                return BadRequest($"Update failed!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // devices controller
        [HttpGet("devices/{deviceId}")]
        public async Task<ActionResult> GetDeviceById(int deviceId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId.IsNullOrEmpty()) return Unauthorized("User not authenticated!");
            var res = await _unitOfWork.Users.GetDevice(Int32.Parse(userId!), deviceId);
            if(res == null) return NotFound(new {message =$"User does not have this deviceID: {deviceId}!"});
            return Ok(res);
        }
        [HttpGet("devices/name")]
        public async Task<ActionResult> GetDeviceByName(string deviceName)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var device = _unitOfWork.Devices.GetByPropertyAsync("DeviceName", deviceName);
            if (userId.IsNullOrEmpty()) return Unauthorized("User not authenticated!");
            var res = await _unitOfWork.Users.GetDevice(Int32.Parse(userId!), device.Id);
            if(res == null) return NotFound(new {message =$"User does not have this deviceID: {device.Id}!"});
            return Ok(res);
        }
        [HttpPost("devices/{deviceId}/add-password")]
        public async Task<ActionResult> AddPasswordToDevice(int deviceId, string password)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            await _unitOfWork.Users.AddPasswordToDevice(deviceId, userId, password);
            return Ok(new { message = "Adding password successfully!" });
        }

        [HttpPut("devices/{deviceId}/change-password")]
        public async Task<ActionResult> ChangePasswordFromDevice(int deviceId, string newPassword)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            await _unitOfWork.Users.ChangePasswordFromDevice(deviceId, userId, newPassword);
            return Ok(new { message = "Changing password successfully!" });
        }

        [HttpDelete("devices/{deviceId}/remove-password")]
        public async Task<ActionResult> RemovePasswordFromDevice(int deviceId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            await _unitOfWork.Users.RemovePasswordFromDevice(deviceId, userId);
            return Ok(new { message = "Removing password successfully!" });
        }

        [HttpGet("devices")]
        public async Task<ActionResult> GetAllDevices()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId.IsNullOrEmpty()) return Unauthorized("User not authenticated!");
            var res = await _unitOfWork.Users.GetAllDevices(Int32.Parse(userId!));
            if (res == null) return NotFound(new { message = $"User does not have any devices!" });
            return Ok(res);
        }

        [HttpPost("devices")]
        public async Task<ActionResult> AddDevice(DeviceDTO request)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Devices.GetByPropertyAsync("DeviceName", request.DeviceName!);
            if (device == null)
            {
                var newDevice = new Device
                {
                    DeviceName = request.DeviceName,
                    State = State.OFF,
                };
                await _unitOfWork.Devices.AddAsync(newDevice);
                var DeviceOwns = new UserDevices
                {
                    UserId = userId,
                    DeviceId = newDevice.Id,
                };
                await _unitOfWork.UserDevices.AddAsync(DeviceOwns);
                return Ok(new { message = $"Adding {newDevice.DeviceName} successfully!" , device = newDevice});
            }
            var isOwns = await _unitOfWork.Users.GetDevice(userId, device.Id);
            if (isOwns == null)
            {
                var DeviceOwns = new UserDevices
                {
                    UserId = userId,
                    DeviceId = device.Id,
                };
                await _unitOfWork.UserDevices.AddAsync(DeviceOwns);
                return Ok(new { message = $"Adding {device.DeviceName} successfully!", device = device });
            }
            return BadRequest(new { message = $"{request.DeviceName} has been existed!" });
        }

        [HttpPut("devices/{deviceId}")]
        public async Task<ActionResult> UpdateDevice(int deviceId, DeviceDTO request)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            _mapper.Map(request, device);
            await _unitOfWork.Devices.UpdateAsync(deviceId, device);
            return Ok(new { message = "Updating device successfully!", updateDevice = device});
        }

        [HttpPut("devices/{deviceId}/change-state")]
        public async Task<ActionResult> ChangeStateDevice(int deviceId, State state)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            await _unitOfWork.Devices.ChangeStateDevice(device, state);
            return Ok(new { message = $"{device.DeviceName} is {state.ToString()}!" });

        }

        [HttpGet("devices/{deviceId}/states")]
        public async Task<ActionResult> GetStatesDevice(int deviceId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var device = await _unitOfWork.Users.GetDevice(userId, deviceId);
            if (device == null) return BadRequest(new { message = $"User does not have DeviceId: {deviceId}!" });
            var res = await _unitOfWork.Devices.GetStatesDevice(deviceId, userId);
            if (res.IsNullOrEmpty()) return BadRequest(new { message = $"{device.DeviceName} has never been worked!" });
            return Ok(res.Select(e => new
            {
                DeviceName = e.Device!.DeviceName,
                State = e.State,
                OperatingTime = e.OperatingTime,
            }).ToList());
        }
        [HttpGet("devices/states")]
        public async Task<ActionResult> GetStatesAllDevices()
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.Devices.getStatesAllDevices(userId);
            if (res.IsNullOrEmpty()) return BadRequest(new { message = "The devices have never been worked!" });
            return Ok(res.Select(e => new
            {
                DeviceName = e.Device!.DeviceName,
                State = e.State,
                OperatingTime = e.OperatingTime,
            }).ToList());
        }
        [HttpGet("devices/{deviceId}/history")]
        public async Task<ActionResult> GetHistoryOfDevice(int deviceId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.GetHistoryActiveByDeviceId(deviceId, userId);
            if(res.IsNullOrEmpty()) return BadRequest(new { message = "The device has never been worked!" });
            return Ok(res.Select(e => new
            {
                DeviceName = e.Device!.DeviceName,
                State = e.State,
                OperatingTime = e.OperatingTime,
            }).ToList());
        }
        [HttpGet("devices/active")]
        public async Task<ActionResult> GetAllDevicesActive()
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.Devices.getActiveAllDevices(userId);
            if (res.IsNullOrEmpty()) return BadRequest(new { message = "Are not have any device working now!" });
            return Ok(res.Select(e => new
            {
                DeviceName = e.DeviceName,
                State = e.State,
            }).ToList());
        }

        [HttpGet("devices/{deviceId}/totalOperatingWorkInDay")]
        public async Task<ActionResult> GetTotalOperatingWorkOfDeviceInDay(int deviceId, DateTime date)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeDeviceWorkingTotalInDay(deviceId, userId, date);
            if (res == 0) return BadRequest(new { message = "The device does not work in this day!" });
            return Ok(new {deviceId = deviceId, totalTime = res });
        }
        [HttpGet("devices/{deviceId}/totalOperatingWorkInWeek")]
        public async Task<ActionResult> GetTotalOperatingWorkOfDeviceInWeek(int deviceId, int week, int year)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeDeviceWorkingTotalInWeek(deviceId, userId, week, year);
            if (res == 0) return BadRequest(new { message = "The device does not work in this week!" });
            return Ok(new { deviceId = deviceId, totalTime = res });
        }
        [HttpGet("devices/{deviceId}/totalOperatingWorkInMonth")]
        public async Task<ActionResult> GetTotalOperatingWorkOfDeviceInMonth(int deviceId, int month, int year)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeDeviceWorkingTotalInMonth(deviceId, userId, month, year);
            if (res == 0) return BadRequest(new { message = "The device does not work in this month!" });
            return Ok(new { deviceId = deviceId, month = month, year = year, totalTime = res });
        }

        [HttpGet("devices/totalOperatingWorkInDay")]
        public async Task<ActionResult> GetTotalOperatingWorkInDay(DateTime date)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeWorkingTotalInDay(userId, date);
            if (res == 0) return BadRequest(new { message = "Does not have any devices work in this day!" });
            return Ok(new {totalTime = res });
        }
        [HttpGet("devices/totalOperatingWorkInWeek")]
        public async Task<ActionResult> GetTotalOperatingWorkInWeek(int week, int year)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeWorkingTotalInWeek(userId, week, year);
            if (res == 0) return BadRequest(new { message = "Does not have any devices work in this week!" });
            return Ok(new {totalTime = res });
        }
        [HttpGet("devices/totalOperatingWorkInMonth")]
        public async Task<ActionResult> GetTotalOperatingWorkInMonth(int month, int year)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.OperateTimeWorkings.TimeWorkingTotalInMonth(userId, month, year);
            if (res == 0) return BadRequest(new { message = "Does not have any devices work in this month!" });
            return Ok(new { month = month, year = year, totalTime = res });
        }
    }
}
