﻿using APIServerSmartHome.Data;
using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace APIServerSmartHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TempHumidController : ControllerBase
    {
        private readonly SmartHomeDbContext _dbContext;
        public TempHumidController(SmartHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost] 
        public async Task<ActionResult> PostTempandHumid(TempHumidDTO request)
        {
            var value = new TempHumidValue
            {
                Temperature = request.Temperature,
                Humidity = request.Humidity,
                TimeSpan = request.TimeSpan
            };
            await _dbContext.TempHumidValues.AddAsync(value);
            await _dbContext.SaveChangesAsync();
            return Ok(value);
        }

        [HttpGet("last-value")]
        public async Task<ActionResult> GetLastTempandHumid()
        {
            var res = await _dbContext.TempHumidValues.OrderBy(x => x.TimeSpan).FirstOrDefaultAsync();
            return Ok(res);
        }
        [HttpGet("all-values")]
        public async Task<ActionResult> GetAllTempandHumid()
        {
            var res = await _dbContext.TempHumidValues.OrderBy(x => x.TimeSpan).ToListAsync();
            return Ok(res);
        }
    }
}
