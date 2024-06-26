﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Customers.CustomerCommand;
using Application.Customers.CustomerQuery;
using Application.DTO;
using Application.Customers.Command;
using Microsoft.AspNetCore.Authorization;
using Application.Customers.Query;
using Application.ResultType;
using Application.TransactionHistory;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Bank/Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Login Here.
        /// </summary>
        ///  /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /Login
        ///     {
        ///         "email": "alexdaniel33@gmail.com",
        ///         "password": "gejsi12",
        ///     }
        /// </remarks>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                Result login = await _mediator.Send(new LoginCustomerCommand(loginDTO.Email, loginDTO.Password));
                if(!login.Succeeded)
                {
                    return Unauthorized();
                }
                return Ok(login);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message.ToString()));
            }
        }
        /// <summary>
        /// Logout here
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Result response = await _mediator.Send(new LogoutCustomerCommand());
                return Ok(response);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message));
            }
        } 

        /// <summary>
        /// Create a Customer here
        /// </summary>
        /// <remarks> NOTE: For Account Type:<br/> 
        ///                 1= Savings<br/>
        ///                 2= Current<br/>
        /// Sample Request:
        /// 
        ///     POST /Customer
        ///     {
        ///         "firstName": "Alex",
        ///         "lastName": "Daniel",
        ///         "email": "alexdaniel33@gmail.com",
        ///         "address": "22, west avenue",
        ///         "phoneNumber": "0812345675",
        ///         "password": "gejsi12",
        ///         "accountType": 1
        ///     }
        ///     </remarks>
        /// <param name="customer"></param>
        /// <returns> This endpoint returns the Customer you just created </returns>
        [HttpPost]
        //Create a Customer
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO customer)
        {
            try
            {
                if (customer == null) { return base.BadRequest(Result.Failure("Invalid customer request")); }
                Result createCustomer = await _mediator.Send(new CreateCustomerCommand(customer));
                if (!createCustomer.Succeeded)
                {
                    return BadRequest(createCustomer);
                }
                return Ok(createCustomer);
                
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message.ToString()));
            }

        }
        
        /// <summary>
        /// Update a Customer here 
        /// </summary>
        /// <remarks>
        /// NOTE: All fields have to be updated
        /// 
        /// Sample Request:
        ///     
        ///     PUT /Customer
        ///     {
        ///        "firstName": "Alex",
        ///         "lastName": "Daniel",
        ///         "email": "alexdaniel33@gmail.com",
        ///         "address": "22, west avenue",
        ///         "phoneNumber": "0812345675"  
        ///     }
        /// </remarks>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut]
        //Update Customer
        public async Task<IActionResult> UpdateCustomer(Guid customerId, CustomerDTO customer)
        {
            try
            {

                Result updateCustomer = await _mediator.Send(new UpdateCustomerCommand(customerId, customer));
                if(!updateCustomer.Succeeded)
                {
                    return BadRequest(updateCustomer);
                }
                return Ok(updateCustomer);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message.ToString()));
            } 
        }
        /// <summary>
        /// View all Customers here 
        /// </summary>
        /// <returns>This endpoint returns all the Customers you have created</returns>
        [HttpGet]
        //View customers 
        public async Task<IActionResult> ViewCustomers()
        {
            try
            {
                Result viewCustomer = await _mediator.Send( new ViewCustomersQuery());
                if(viewCustomer.Message== "No customer has been created")
                {
                    return BadRequest(viewCustomer);
                }
                return Ok(viewCustomer);

            }
            catch (Exception exception)
            {

                return base.BadRequest(Result.Failure(exception.Message));
            }

        }
        /// <summary>
        /// View customer by ID here
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Returns the specific customer you're looking for</returns>
        [HttpGet("{customerId}")]
        public async Task<IActionResult> ViewCustomerById(Guid customerId)
        {
            try
            {
                if (customerId == Guid.Empty)
                {
                    return base.BadRequest(Result.Failure("Field cannot be empty"));
                }
                Result viewCustomerById = await _mediator.Send(new ViewCustomerByIdQuery(customerId));
                if (!viewCustomerById.Succeeded)
                {
                    return BadRequest(viewCustomerById);
                }
                else
                {
                    return Ok(viewCustomerById);
                }

            }
            catch (Exception ex)
            {
                return base.BadRequest(Result.Failure(ex.Message));
                
            }
        }
        /// <summary>
        /// View Customers Transaction History here
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Returns the specified customers transaction history</returns>
        [HttpGet("/transaction")]
        public async Task<IActionResult> ViewTransactionHistoryById(Guid customerId)
        {
            try
            {
                if (customerId == Guid.Empty)
                {
                    return base.BadRequest(Result.Failure("Field cannot be empty"));
                }
                Result response = await _mediator.Send(new ViewCustomerTransaction_HistoryByIdQuery(customerId));
                if (!response.Succeeded)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message));
            }
        }
        [HttpDelete("/deleteTransaction")]
        public async Task<IActionResult> DeleteTrancactionHistory()
        {
            try
            {
                Result deletedCustomer = await _mediator.Send(new DeleteTransactionHistoryCommand());
                if (!deletedCustomer.Succeeded)
                {
                    return BadRequest(deletedCustomer);
                }
                return Ok(deletedCustomer);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message));
            }

        }

        /// <summary>
        /// Delete Customers here
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// <repsonse code= '200'> Returns a success response</repsonse>
        /// <response code= '400'>If the Customer does not exist</response>
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            try
            {
                Result deletedCustomer = await _mediator.Send(new DeleteCustomerCommand(customerId));
                if (!deletedCustomer.Succeeded)
                {
                    return BadRequest(deletedCustomer);
                }
                return Ok(deletedCustomer);
            }
            catch (Exception ex)
            {

                return base.BadRequest(Result.Failure(ex.Message));
            }

        }
    }
}
