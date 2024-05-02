using Application.RefreshTokens;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Mini_Core_Banking_Project.Controllers
{
    [ApiController]
    [Route("api/Bank/RefreshToken")]
    public class RefreshTokenController : Controller
    {
        private readonly IMediator _mediator;
        public RefreshTokenController(IMediator mediator) 
        { 
            _mediator = mediator;
        }
        /// <summary>
        /// Refresh your Access Token here. 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RefreshAccessToken([FromBody]string refreshToken)
        {
            try
            {
                AuthenticatedResponse response = await _mediator.Send(new RefreshAccessTokenCommand(refreshToken));
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);

            }
            catch (Exception ex)
            {

                return BadRequest(new AuthenticatedResponse { Message = ex.Message, Success=false });
            }
        }
    }
}
