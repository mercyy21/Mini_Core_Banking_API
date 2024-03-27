using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Customers.CustomerCommand;
using Application.Customers.CustomerQuery;
using Domain.DTO;

namespace Mini_Core_Banking_Project.Controllers
{
    [ApiController]
    [Route("api/Bank/Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            this._mediator = mediator;
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
                if (customer == null) { return BadRequest(new ResponseModel { Message = "Invalid customer request", Success = false}); }
                MultipleDataResponseModel createCustomer = await _mediator.Send(new CreateCustomerCommand(customer));
                if (!createCustomer.Success)
                {
                    return BadRequest(createCustomer);
                }
                return Ok(createCustomer);
                
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Message = ex.Message.ToString(), Success= false });
            }

        }/// <summary>
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
                
                ResponseModel updateCustomer = await _mediator.Send(new UpdateCustomerCommand(customerId,customer));
                if(!updateCustomer.Success)
                {
                    return BadRequest(updateCustomer);
                }
                return Ok(updateCustomer);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Message = ex.Message.ToString(),Success= false});
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
                ResponseModel viewCustomer = await _mediator.Send( new ViewCustomersQuery());
                if(viewCustomer.Message== "No customer has been created")
                {
                    return BadRequest(viewCustomer);
                }
                return Ok(viewCustomer);

            }
            catch (Exception exception)
            {

                return BadRequest(new ResponseModel { Message = exception.Message, Success=false });
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
                    return BadRequest("Field cannot be empty");
                }
                ResponseModel viewCustomerById = await _mediator.Send(new ViewCustomerByIdQuery(customerId));
                if (!viewCustomerById.Success)
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
                return BadRequest(new ResponseModel { Message = ex.Message, Success= false });
                
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
                ResponseModel deletedCustomer = await _mediator.Send(new DeleteCustomerCommand(customerId));
                if (!deletedCustomer.Success)
                {
                    return BadRequest(deletedCustomer);
                }
                return Ok(deletedCustomer);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Message = ex.Message, Success = false });
            }

        }
    }
}
