using backend.Recycle.Abstracts.Repositories;
using backend.Recycle.Data;
using backend.Recycle.Data.Models;
using backend.Recycle.Data.ViewModels;
using backend.Recycle.Data.Enums;
using backend.Recycle.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace backend.Recycle.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private REGISTERDbContext _ctx;
        private UserManager<Users> _userManager;
        private readonly IUserRepository _user;
        private readonly IRequestRepository _request;
        public UserController(REGISTERDbContext ctx, UserManager<Users> userManager,IUserRepository user,IRequestRepository request)
        {
            _ctx = ctx;
            _userManager = userManager;
            _user = user;
            _request = request;
        }
        [Authorize(Roles = "hyperVisor")]
        [HttpPost]

        [Route(nameof(SetEmployee))]
        

        public async Task<IActionResult> SetEmployee([FromBody] SetEmployeeRequest request)

        {
            await _ctx.Database.BeginTransactionAsync();
            var userId = User.GetUserId();
            var user = _ctx.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null) return NotFound();
            var result = await _userManager.AddToRoleAsync(user, "employee");
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var addEmployeeToZone = await _user.SetAvailabilityEmployee(request.UserId, request.AvailabilityZoneId);
           await _ctx.Database.CommitTransactionAsync();
            if (addEmployeeToZone == false)
                return BadRequest("Can't add Employee To Zone");
            return Ok();
        }

        [Authorize]
        [Route(nameof(GetUserRequest))]
        [HttpGet]
        public async Task<ActionResult> GetUserRequest()
        {
            var UserId = User.GetUserId();

            if (UserId == null)
            {
                return BadRequest();
            }
            var data = _ctx.Requests.Where(e => e.UserId == UserId).AsEnumerable();

            if (data.Count() <= 0)
            {
                return NotFound("no requestes");
            }

            return Ok(data);
        }

        editRequest r2 = new editRequest();
        RequestEntity r1 = new RequestEntity();

        [HttpPut]
        public async Task<IActionResult> editUserRequest([FromQuery] string photoUrl, [FromQuery] size size, [FromQuery] rubbishType rubbishType)
        {
            DateTime editBaseTime = DateTime.Now;
            var requestId = r1.Id;
            var userUpdate = new ReceivedRequest();

            if (requestId == null && editBaseTime.Day >= r1.requestDateTime.Day && !ModelState.IsValid)
                return BadRequest();
            r2.photoUrl = photoUrl;
            r2.size = size;
            r2.rubbishType = rubbishType;
            userUpdate.Request.photoUrl = r2.photoUrl;
            userUpdate.Request.rubbishType = r2.rubbishType;
            userUpdate.Request.size = r2.size;
            return Ok("updated");
        }

        ReceivedRequest ERequest = new ReceivedRequest();
        /*api=>getEmployeeRequests return list of datetime+clientname+fullAddress*/
        [HttpGet("{ERequest.Id}")]
        public async Task<IActionResult> getEmployeeRequest()
        {
            if (ERequest.Id == null && ERequest.RequestId == null)
                return BadRequest();
            string dateTime = ERequest.Request.requestDateTime.ToString();
            var clientname = ERequest.Request.User.SSID;
            var fullAddress = ERequest.Request.User.Address;
            String []recievedRequests = { dateTime, clientname, fullAddress };
            return Ok(recievedRequests);
        }

        [HttpPost()]
        [Route(nameof(PostEmployeeRequest))]
        public async Task<IActionResult> PostEmployeeRequest([FromBody] UserRequestModel model)
        {
            var userId = User.GetUserId();
            var request = await _request.PostRequest(model,userId);
            if (!request)
            {
                return BadRequest("Can't Post Request");
            }
            return Ok();
        }
    }
}