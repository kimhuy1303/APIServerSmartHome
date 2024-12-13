using APIServerSmartHome.DTOs;
using APIServerSmartHome.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]       
        public async Task<ActionResult> GetUserById (int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if(user == null) return NotFound(new {message = "User does not exist!"});
            return Ok(user);
        }
        [HttpPut("update-infomation-user-by-id/{id}")]
        public async Task<ActionResult> UpdateProfile(int id ,ProfileDTO profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existing_user = await _unitOfWork.Users.GetByIdAsync(id);
                    if (existing_user != null)
                    {
                        _mapper.Map(profile, existing_user);
                    }
                    await _unitOfWork.Users.UpdateAsync(id, existing_user);
                    return Ok(new {message = "Update infomation successfully!", user = existing_user });
                }
                return BadRequest($"User id [{id}] Not Found");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
            
        }
    }
}
