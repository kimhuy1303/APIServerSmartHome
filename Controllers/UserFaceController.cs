using APIServerSmartHome.DTOs;
using APIServerSmartHome.Entities;
using APIServerSmartHome.Services;
using APIServerSmartHome.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIServerSmartHome.Controllers
{
    [Route("api/User/")]
    [ApiController]
    [Authorize]
    public class UserFaceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FaceRecognitionService _faceRecognitionService;
        public UserFaceController(IUnitOfWork unitOfWork, FaceRecognitionService faceRecognitionService)
        {
            _unitOfWork = unitOfWork;
            _faceRecognitionService = faceRecognitionService;
        }

        [HttpPost("user-faces")]
        public async Task<ActionResult> UploadUserFace(UserFaceDTO request) 
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var newUserFace = new UserFaces
            {
                Name = request.Name,
                FaceImage = request.FaceImage,
                CreatedAt = DateTime.Now,
                UserId = userId,
            };
            await _unitOfWork.UserFaces.AddAsync(newUserFace);
            return Ok(new { message = "Uploads user face image successfully!" });
        }

        [HttpDelete("user-faces/{userFaceId}")]
        public async Task<ActionResult> RemoveUserFace(int userFaceId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var userFace = await _unitOfWork.UserFaces.GetFaceDataOfUser(userFaceId, userId);
            if (userFace == null) return NotFound(new { message = $"Face data does not exist!" });
            await _unitOfWork.UserFaces.DeleteAsync(userFace.Id);
            return Ok(new { message = "Removing face data successfully!" });
        }

        [HttpPost("user-faces/verify")]
        public async Task<ActionResult> VerifyFaceImage(FaceVerificationDTO request)
        {
            var input = Convert.FromBase64String(request.FaceImg!);

            var facesData = await _unitOfWork.UserFaces.GetAllAsync();

            foreach(var face in facesData)
            {
                var dataImgBytes = Convert.FromBase64String(face.FaceImage!);

                var similarity = _faceRecognitionService.CompareFacesData(input, dataImgBytes); 

                if(similarity < 50)
                {
                    return Ok(new { message = "Face recognized!", Username = face.Name });
                }
            }
            return BadRequest(new { messaage = "Face not recognized!" });
        }

        [HttpGet("user-faces")]
        public async Task<ActionResult> GetAll()
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var res = await _unitOfWork.UserFaces.GetAllFaceDataOfUser(userId);
            if (res.IsNullOrEmpty()) return NotFound(new { message = "User does not have any face datas" });
            return Ok(res);
        }
        [HttpGet("user-faces/{faceId}")]
        public async Task<ActionResult> GetFaceData(int faceId)
        {
            var userId = Int32.Parse(User.FindFirst("UserId")?.Value!);
            var userFace = await _unitOfWork.UserFaces.GetFaceDataOfUser(faceId, userId);
            if (userFace == null) return NotFound(new { message = "Face data does not exist!" });
            return Ok(userFace);
        }
    }
}
