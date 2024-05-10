using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;
using Application.Accounts.AccountCommand;
using Application.Accounts.AccountQuery;
using Microsoft.AspNetCore.Authorization;
using Application.Accounts.Command;
using Application.ResultType;


namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Bank/Account")]
    public class AccountController : Controller
    {

        private readonly IMediator _mediator;

        public AccountController( IMediator mediator)
        {
            _mediator = mediator;
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
                if (account == null) { return base.BadRequest(Application.ResultType.Result.Failure("Invalid account request")); }
                Application.ResultType.Result createAccount =await _mediator.Send(new CreateAccountCommand(account));
                if (!createAccount.Succeeded)
                {
                    return BadRequest(createAccount);
                }
                return Ok(createAccount);
            }
            catch (Exception ex)
            {
                return base.BadRequest(Application.ResultType.Result.Failure( ex.Message.ToString()));

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
        /// <param name="transactDTO"></param>
        /// <returns></returns>
        [HttpPost("/Deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactDTO transactDTO)
        {
            try
            {
                Application.ResultType.Result response = await _mediator.Send(new DepositCommand(transactDTO));
                if (!response.Succeeded)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(ex.Message.ToString()));
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
        /// <param name="transactDTO"></param>
        /// <returns>This endpoints returns Amount exceeds balance or Customer does not exist when it's a bad request</returns>
        [HttpPost("/Withdraw")]
        public async Task<IActionResult> Withdraw(TransactDTO transactDTO)
        {
            Application.ResultType.Result response = await _mediator.Send(new WithdrawCommand(transactDTO));
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        /// <summary>
        /// Transfer Money to Customers Account
        /// </summary>
        /// <remarks>NOTE: 
        ///                Account Number cannot be null.<br/>
        ///                Amount cannot be null.<br/>
        ///                Amount has to be greater than zero.<br/>
        /// Sample request:
        /// 
        ///     POST /Transfer
        ///     {
        ///         "accountNumber": "9459532478",
        ///         "amount": 1000
        ///     }
        /// </remarks>
        /// <param name="transfer"></param>
        /// <returns></returns>
        [HttpPost("/Transfer")]
        public async Task<IActionResult> Transfer(TransferDTO transfer)
        {
            try
            {
                Application.ResultType.Result message = await _mediator.Send(new TransferCommand(transfer));
                if (!message.Succeeded)
                {
                    return BadRequest(message);
                }
                return Ok(message);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(ex.Message.ToString()));
            }
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
                Application.ResultType.Result existingAccount =await _mediator.Send(new ViewCustomersAccountQuery(customerId));
                if (existingAccount.Entity == null)
                {
                    return NotFound(existingAccount);
                }
                return Ok(existingAccount);
            }
            catch (Exception exception)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(exception.Message.ToString()));
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
                Application.ResultType.Result activatedCustomer = await _mediator.Send(new ActivateAccountCommand(accountId));
                if (!activatedCustomer.Succeeded)
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
                return base.BadRequest(Application.ResultType.Result.Failure( exception.Message));
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
                Application.ResultType.Result deactivatedCustomer =await _mediator.Send(new DeactivateAccountCommand(accountId));
                if (!deactivatedCustomer.Succeeded)
                {
                    return BadRequest(deactivatedCustomer);
                }
                return Ok(deactivatedCustomer);
            }
            catch (Exception exception)
            {

                return base.BadRequest(Application.ResultType.Result.Failure(exception.Message.ToString()));
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
                Application.ResultType.Result response =await _mediator.Send(new DeleteAccountCommand(customerId));
                if(!response.Succeeded)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return base.BadRequest(Application.ResultType.Result.Failure(ex.Message.ToString()));
            }
        }
    }
}
