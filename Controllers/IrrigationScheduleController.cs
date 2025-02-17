using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IrrigationScheduleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public IrrigationScheduleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> AddSchedule(IrrigationScheduleDTO request)
        {
            var schedule = new IrrigationSchedule
            {
                TimeWorking = request.TimeWorking,
                IsActive = false,
            };
            await _unitOfWork.IrrigationSchedules.AddSchedule(schedule);
            return Ok(new { message = "Add schedule succesfully!", schedule = schedule });
        }

        [HttpGet]
        public async Task<ActionResult> GetSchedule()
        {
            var schedule = await _unitOfWork.IrrigationSchedules.GetSchedule();
            if(schedule == null) { return NotFound(); }
            return Ok(schedule);
        }

        [HttpPut("{scheduleId}/handle-active")]
        public async Task<ActionResult> HandleActive(int scheduleId)
        {
            var schedule = await _unitOfWork.IrrigationSchedules.GetScheduleById(scheduleId);
            await _unitOfWork.IrrigationSchedules.HandleActive(schedule);
            return Ok(schedule);
        }

        [HttpPut("{scheduleId}/change-timeworking")]
        public async Task<ActionResult> ChangeTimeWorking(int scheduleId, DateTime timeworking)
        {
            var schedule = await _unitOfWork.IrrigationSchedules.GetScheduleById(scheduleId);
            await _unitOfWork.IrrigationSchedules.ChangeTimeWorking(schedule, timeworking);
            return Ok(schedule);
        }
    }
}
