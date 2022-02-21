using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.TransactionCommands;
using MoneyTracker.Application.TransactionQueries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoneyTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : BaseController
    {
        // GET: api/<TransactionController>
        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAll(string? accountName,string? tagName,DateTime? date)
        {
            return Ok(await Mediator.Send(new GetAllFilterTransactionsQuery 
            {
                AccountName=accountName,
                TagName=tagName,
                Date=date
            }));
        }

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetTransactionQuery { Id = id }));
        }

        // POST api/<TransactionController>
        [HttpPost]
        public async Task<IActionResult> Add(AddTransactionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteTransactionCommand { Id = id });

            return NoContent();
        }
    }
}
