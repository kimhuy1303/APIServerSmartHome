using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIServerSmartHome.Controllers
{
    [Route("api/User/")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("rooms")]
        public async Task<ActionResult> GetAllRooms()
        {
            var result = await _unitOfWork.Rooms.GetAllAsync();
            if (result.IsNullOrEmpty()) return NotFound(new { message = "Does not have any rooms!" });
            return Ok(result);
        }
        [HttpGet("rooms/{roomId}")]
        public async Task<ActionResult> GetRoom(int roomId)
        {
            var result = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (result == null) return NotFound(new { message = $"Does not have this roomID: {result.Id}!" });
            return Ok(result);
        }

        [HttpPost("rooms")]
        public async Task<ActionResult> AddRoom(RoomDTO request)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetByPropertyAsync("RoomName",request.RoomName!);
                if (room == null)
                {
                    var newRoom = new Room
                    {
                        RoomName = request.RoomName,
                    };
                    await _unitOfWork.Rooms.AddAsync(newRoom);
                    return Ok(new { message = "Add new room successfully!", newRoom = newRoom });
                }
                return BadRequest("Room has existed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("rooms/{roomId}/devices")]
        public async Task<ActionResult> GetAllDeviceInSite(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null) return NotFound(new { message = $"RoomID: {id} does not exist!" });
            var res = await _unitOfWork.Rooms.GetAllDevicesInSite(id);
            if (res.IsNullOrEmpty()) return NotFound(new {message = $"{room.RoomName} does not have any devices!"});
            return Ok(res);
        }

        [HttpPost("rooms/{roomId}/devices/{deviceId}")]
        public async Task<ActionResult> AddDeviceIntoSite(int roomId, int deviceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            var device = await _unitOfWork.Devices.GetByIdAsync(deviceId);
            if (device == null) return BadRequest(new { message = $"Device {deviceId} does not exist!" });
            if (room == null) return BadRequest(new { message = $"Site {roomId} does not exist!" });
            await _unitOfWork.Rooms.AddDeviceIntoSite(device, room.Id);
            return Ok(new { message = $"Adding {device.DeviceName} into {room.RoomName} successfully!", device = device });
        }

        [HttpDelete("rooms/{roomId}/devices/{deviceId}\"")]
        public async Task<ActionResult> RemoveDeviceFromSite(int roomId, int deviceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            var device = await _unitOfWork.Devices.GetByIdAsync(deviceId);
            if (device == null) return BadRequest(new { message = $"Device {deviceId} does not exist!" });
            if (room == null) return BadRequest(new { message = $"Site {roomId} does not exist!" });
            await _unitOfWork.Rooms.RemoveDeviceFromSite(roomId, deviceId);
            return Ok(new { message = $"Removing {device.DeviceName} from {room.RoomName} successfully!" });
        }

        [HttpDelete("rooms/{roomId}")]
        public async Task<ActionResult> DeleteSite(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return BadRequest(new { message = $"Site {roomId} does not exist!" });
            await _unitOfWork.Rooms.DeleteAsync(roomId);
            return Ok(new { message = $"Delete {room.RoomName} successfully!" });
        }
        [HttpGet("rooms/{roomId}/devices/amount")]
        public async Task<ActionResult> GetTotalDevicesInSite(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return NotFound(new { message = $"RoomID: {roomId} does not exist!" });
            var res = await _unitOfWork.Rooms.GetAllDevicesInSite(roomId);
            return Ok(new { amount = res.Count });
        }

        [HttpGet("rooms/{roomId}/devices/active")]
        public async Task<ActionResult> GetActiveDevicesInSite(int roomId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.Devices.getActiveDevicesBySite(userId, roomId);
            if (res.IsNullOrEmpty()) return BadRequest(new { message = "Do not have any device active now!" });
            return Ok(res.Select(e => new
            {
                DeviceName = e.DeviceName,
                State = e.State,
                Site = e.Room!.RoomName,
            }).ToList());
        }
    }
}
