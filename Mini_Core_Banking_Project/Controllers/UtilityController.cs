using Application.DTO;
using Application.ResultType;
using Application.Utility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Bank/Utility")]
    public class UtilityController: Controller
    {
        private readonly IMediator _mediator;

        public UtilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create Signature for Deposit and Withdraw here
        /// </summary>
        /// <param name="signatureDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Signature([FromBody] TransactionDetailsDTO signatureDTO)
        {
            try
            {
                Application.ResultType.Result response = await _mediator.Send(new EncryptCommand(signatureDTO));
                return Ok(response);

            }
            catch (Exception ex)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(ex.Message.ToString()));
            }
        }
        [HttpPost("/Decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] string signature)
        {
            try
            {
                Application.ResultType.Result response = await _mediator.Send(new DecryptCommand(signature));
                if (!response.Succeeded)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(ex.Message));
            }
        }
    }
}
