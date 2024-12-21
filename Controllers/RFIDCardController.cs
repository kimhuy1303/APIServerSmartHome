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

        [HttpPut("rfidcards/{cardId}/active")]
        public async Task<ActionResult> ActiveRfidCard(int cardId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId, userId);
            if (card == null) return NotFound(new { message = "Card not found" });
            if (card.IsActive == true) return BadRequest(new { message = "Active card failed!" });
            await _unitOfWork.Cards.ChangeActiveState(card, true);
            return Ok(new { message = "Active card successfully!"});

        }
        [HttpPut("rfidcards/{cardId}/inactive")]
        public async Task<ActionResult> InactiveRfidCard(int cardId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId, userId);
            if (card == null) return NotFound(new { message = "Card not found" });
            if (card.IsActive == false) return BadRequest(new { message = "Inactive card failed!" });
            await _unitOfWork.Cards.ChangeActiveState(card, false);
            return Ok(new { message = "Inactive card successfully!"});
        }
        [HttpPut("rfidcards/{cardId}/grant-permission")]
        public async Task<ActionResult> GrantAccessLevelForCard(int cardId, AccessLevel accessLevel)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var card = await _unitOfWork.Cards.GetCardByUser(cardId, userId);
            if (card == null) return NotFound(new { message = "Card not found" });
            if (card.AccessLevel == accessLevel) return BadRequest(new { message = "Grant permission failed!" });
            await _unitOfWork.Cards.GrantAccessLevel(card, accessLevel);
            return Ok(new {message = "Grant permission successfully", permission = card.AccessLevel });
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
