using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRooms()
        {
            var result = await _unitOfWork.Rooms.GetAllAsync();
            if (result.IsNullOrEmpty()) return NotFound(new { message = "Does not have any rooms!" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoom(RoomDTO request)
        {
            try
            {
                var room = await _unitOfWork.Rooms.GetByNameAsync(request.RoomName);
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
    }
}
