using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Enum;
using APIServerSmartHome.UnitOfWorks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServerSmartHome.Controllers
{
    [Route("api/User/")]
    [ApiController]
    [Authorize]
    public class RFIDCardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RFIDCardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("rfidcards")]
        public async Task<ActionResult> AddRfidCard(RFIDCardDTO request)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var check = await _unitOfWork.Cards.GetCardByUser(request.CardUID!, userId);
            if (check != null) return BadRequest(new { message = "Card has been existed!" });
            var newCard = new RFIDCard
            {
                CardUID = request.CardUID,
                AccessLevel = Enum.AccessLevel.NoAccept,
                IsActive = true,
                UserId = userId,
            };
            await _unitOfWork.Cards.AddAsync(newCard);
            return Ok(new {message = "Adding rfid card successfully!"});
        }

        [HttpPut("rfidcards/{cardId}")]
        public async Task<ActionResult> UpdateRfidCard(int cardId, RFIDCardUpdateDTO request)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId, userId);
            if (card == null) return NotFound(new { message = "Card not found" });
            card.CardUID = request.CardUID;
            card.AccessLevel = request.AccessLevel;
            card.IsActive = request.IsActive;
            await _unitOfWork.Cards.UpdateAsync(cardId,card);
            return Ok(new { message = "Updating rfid card successfully!" });
        }

        [HttpGet("rfidcards")]
        public async Task<ActionResult> GetAllCardsByUser()
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var cards = await _unitOfWork.Cards.GetCardsByUser(userId);
            return Ok(cards);
        }
        [HttpGet("rfidcards/{cardId}")]
        public async Task<ActionResult> GetCardByUser(int cardId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId,userId);
            return Ok(card);
        }
        [HttpDelete("rfidcards/{cardId}")]
        public async Task<ActionResult> DeleteCardByUser(int cardId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId,userId);
            await _unitOfWork.Cards.DeleteAsync(card.Id);
            return Ok(new {message = "Deleting card successfully!"});
        }
    }
}
