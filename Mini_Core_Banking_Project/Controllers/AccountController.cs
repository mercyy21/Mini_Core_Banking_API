using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.DTO;
using Application.Accounts.AccountCommand;
using Application.Accounts.AccountQuery;


namespace Mini_Core_Banking_Project.Controllers
{
    [ApiController]
    [Route("api/Bank/Account")]
    public class AccountController : Controller
    {

        private readonly IMediator _mediator;

        public AccountController( IMediator mediator)
        {
            this._mediator = mediator;
        }


        /// <summary>
        /// Create an account here.
        /// </summary>
        /// <remarks> NOTE:
        ///                 Null values are not accepted<br/>
        ///                 For Account Type:<br/> 
        ///                 1= Savings<br/>
        ///                 2= Current<br/>
        ///               
        /// Sample request:
        /// 
        ///     POST /Account
        ///     {
        ///         "customerId": "79b0daeb-ae9b-4556-b5ac-cd26112d03ea",
        ///         "accountType": 1
        ///     }
        ///      </remarks>
        /// <param name="account"></param>
        /// <returns> This endpoint return the Account you just created</returns>
        [HttpPost]
        //Create an Account for a customer
        public async Task<IActionResult> CreateAccount(AccountDTO account)
        {
            try
            {
                if (account == null) { return BadRequest(new ResponseModel { Message = "Invalid account request", Success = false }); }
                ResponseModel createAccount =await  _mediator.Send(new CreateAccountCommand(account));
                if (!createAccount.Success)
                {
                    return BadRequest(createAccount);
                }
                return Ok(createAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message.ToString() });

            }

        }

        /// <summary>
        /// Deposit Money into Customers Account
        /// </summary>
        /// <remarks>NOTE: 
        ///                Account ID cannot be null.<br/>
        ///                Amount cannot be null.<br/>
        ///                Amount has to be greater than zero.<br/>
        /// 
        /// Sample request:
        /// 
        ///     POST /Account
        ///     {
        ///         "accountId": "62b0daeb-dc8b-3445-c4db-bc57017d10de",
        ///         "amount": 1000
        ///     }
        /// 
        /// </remarks>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost("/deposit")]
        public async Task<IActionResult> Deposit(Guid accountId, double amount)
        {
            try
            {
                ResponseModel message = await _mediator.Send(new DepositCommand(accountId, amount));
                if (message.Message.Contains("greater"))
                {
                    return BadRequest(message);
                }
                if (message.Message.Contains("exist"))
                {
                    return BadRequest(message);
                }
                return Ok(message);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Message = ex.Message.ToString(), Success = false });
            }

        }

        /// <summary>
        /// Withdraw Money from a Customers Account
        /// </summary>
        /// <remarks>NOTE: 
        ///                Account ID cannot be null.<br/>
        ///                Amount cannot be null.<br/>
        ///                Amount has to be greater than zero.<br/>
        /// Sample request:
        /// 
        ///     POST /Account
        ///     {
        ///         "accountId": "62b0daeb-dc8b-3445-c4db-bc57017d10de",
        ///         "amount": 1000
        ///     }
        /// </remarks>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns>This endpoints returns Amount exceeds balance or Customer does not exist when it's a bad request</returns>
        [HttpPost("/withdraw")]
        public async Task<IActionResult> Withdraw(Guid accountId, double amount)
        {
            ResponseModel message = await _mediator.Send(new WithdrawCommand(accountId, amount));
            if (message.Message.Contains("exceeds") || message.Message.Contains("exist"))
            {
                return BadRequest(message);
            }
            return Ok(message);
        }
        /// <summary>
        /// Get a Customers Account
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>This endpoint returns all exisiting Customer Accounts</returns>
        [HttpGet]
        public async Task<IActionResult> ViewCustomersAccount(Guid customerId)
        {
            try
            {
                ResponseModel existingAccount =await _mediator.Send(new ViewCustomersAccountQuery(customerId));
                if (existingAccount.Data == null)
                {
                    return NotFound(existingAccount);
                }
                return Ok(existingAccount);
            }
            catch (Exception exception)
            {

                return BadRequest(new ResponseModel { Success = false, Message = exception.Message.ToString() });
            }

        }
        /// <summary>
        /// Activate a Customers Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>This endpoint returns </returns>
        [HttpPut("/activate/{accountId}")]
        public async Task<IActionResult> Activate(Guid accountId)
        {
            try
            {
                ResponseModel activatedCustomer = await _mediator.Send(new ActivateAccountCommand(accountId));
                if (activatedCustomer.Success == false)
                {
                    return BadRequest(activatedCustomer);
                }
                else
                {
                    return Ok(activatedCustomer);
                }
            }

            catch (Exception exception)
            {
                return BadRequest(new ResponseModel { Message = exception.Message, Success = false });
            }
        }
        /// <summary>
        /// Deactivate Customers Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut("/deactivate/{accountId}")]
        public async Task<IActionResult> Deactivate(Guid accountId)
        {
            try
            {
                ResponseModel deactivatedCustomer =await  _mediator.Send(new DeactivateAccountCommand(accountId));
                if (deactivatedCustomer.Success == false)
                {
                    return BadRequest(deactivatedCustomer);
                }
                return Ok(deactivatedCustomer);
            }
            catch (Exception exception)
            {

                return BadRequest(new ResponseModel { Message = exception.Message.ToString(), Success = false });
            }


        }
       /// <summary>
       /// Deletes Customers Account
       /// </summary>
       /// <param name="customerId"></param>
       /// <returns></returns>
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAccount(Guid customerId)
        {
            try
            {
                ResponseModel response =await  _mediator.Send(new DeleteAccountCommand(customerId));
                if(!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest( new ResponseModel { Message = ex.Message.ToString(),Success= false});
            }
        }
    }
}
