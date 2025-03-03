using APIServerSmartHome.Data;
using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SoilHumidityController : ControllerBase
    {
        private readonly SmartHomeDbContext _dbContext;
        public SoilHumidityController(SmartHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult> PostSoilHumidity(SoilHumidityDTO request)
        {
            var value = new SoilHumidity
            {
                Value = request.Value,
                TimeSpan = request.TimeSpan
            };
            await _dbContext.SoilHumidities.AddAsync(value);
            await _dbContext.SaveChangesAsync(); 
            return Ok(value);
        }

        [HttpGet("all-values")]
        public async Task<ActionResult> GetAllSoilHumidity()
        {
            var res = await _dbContext.SoilHumidities.OrderBy(x => x.TimeSpan).ToListAsync();
            return Ok(res);
        }

        [HttpGet("last-value")]
        public async Task<ActionResult> GetLastSoilHumidity()
        {
            var res = await _dbContext.SoilHumidities.OrderBy(x => x.TimeSpan).FirstOrDefaultAsync();
            return Ok(res);
        }

        [HttpGet("statistics/day")]
        public async Task<ActionResult> GetStatisticsByDay(DateTime day)
        {
            var res = await _dbContext.SoilHumidities.Where(sh => sh.TimeSpan!.Date == day.Date).ToListAsync();
            return Ok(res);
        }

        [HttpGet("statistics/month")]
        public async Task<ActionResult> GetStatisticsByMonth(int month, int year)
        {
            var res = await _dbContext.SoilHumidities.Where(sh => sh.TimeSpan!.Month == month && sh.TimeSpan!.Year == year).ToListAsync();
            return Ok(res);
        }



    }
}
