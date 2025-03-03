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
            var response = new List<RoomResponseDTO>();
            var result = await _unitOfWork.Rooms.GetAllAsync();
            if (result.IsNullOrEmpty()) return NotFound(new { message = "Does not have any rooms!" });
            foreach (var room in result)
            { 
                var devices = await _unitOfWork.Rooms.GetAllDevicesInSite(room.Id);
                response.Add(new RoomResponseDTO
                {
                    Id = room.Id,
                    RoomName = room.RoomName,
                    AmountOfDevice = devices.Count,
                });
            }
            return Ok(response);
        }
        [HttpGet("rooms/{roomId}")]
        public async Task<ActionResult> GetRoom(int roomId)
        {
            var result = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (result == null) return NotFound(new { message = $"Does not have this roomID: {result!.Id}!" });
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

        [HttpPut("rooms/{roomId}")]
        public async Task<ActionResult> UpdateRoom(int roomId, RoomDTO request)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return NotFound(new { message = $"RoomID: {roomId} does not exist!" });
            room.RoomName = request.RoomName;
            await _unitOfWork.Rooms.UpdateAsync(roomId, room);
            return Ok(new { message = "Update room successfully!", room = room });
        }

        [HttpGet("rooms/{roomId}/devices")]
        public async Task<ActionResult> GetAllDeviceInSite(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) return NotFound(new { message = $"RoomID: {roomId} does not exist!" });
            var res = await _unitOfWork.Rooms.GetAllDevicesInSite(roomId);
            if (res.IsNullOrEmpty()) return NotFound(new {message = $"{room.RoomName} does not have any devices!"});
            return Ok(res.Select(e => new {id = e.Id, deviceName = e.DeviceName, state = e.State}).ToList());
        }

        [HttpPost("rooms/{roomId}/devices/{deviceId}")]
        public async Task<ActionResult> AddDeviceIntoSite(int roomId, int deviceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            var device = await _unitOfWork.Devices.GetByIdAsync(deviceId);

            if (device == null) return BadRequest(new { message = $"Device {deviceId} does not exist!" });
            if (room == null) return BadRequest(new { message = $"Site {roomId} does not exist!" });
            var check = room.Devices.FirstOrDefault(device => device.Id == deviceId);
            if(check != null) return BadRequest(new {message = $"Device {deviceId} has already existed in this site {room.RoomName}" });
            await _unitOfWork.Rooms.AddDeviceIntoSite(device, room.Id);
            return Ok(new { message = $"Adding {device.DeviceName} into {room.RoomName} successfully!", device = device });
        }

        [HttpDelete("rooms/{roomId}/devices/{deviceId}")]
        public async Task<ActionResult> RemoveDeviceFromSite(int roomId, int deviceId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            var device = await _unitOfWork.Devices.GetByIdAsync(deviceId);
            if (device == null) return BadRequest(new { message = $"Device {deviceId} does not exist!" });
            if (room == null) return BadRequest(new { message = $"Site {roomId} does not exist!" });
            var check = room.Devices.FirstOrDefault(device => device.Id == deviceId);
            if (check == null) return BadRequest(new { message = $"Device {deviceId} does not exist in this site {room.RoomName}" });
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
