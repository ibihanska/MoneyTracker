using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.AccountCommands;
using MoneyTracker.Application.AccountQueries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoneyTracker.SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        // GET: api/<AccountsController>
        [HttpGet]
        public async Task<ActionResult<List<AccountDto>>> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllAccountsQuery()));
        }

        // GET: api/<AccountsController>
        [HttpGet("/getInfo")]
        public async Task<ActionResult<List<AccountInfoDto>>> GetInfo()
        {
            return Ok(await Mediator.Send(new GetAccountInfoQuery()));
        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetAccountQuery { Id = id }));
        }

        // POST api/<AccountsController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAccountCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] EditAccountCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteAccountCommand { Id = id });

            return NoContent();
        }
    }
}
