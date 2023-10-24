using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TerraformAPIDotnet.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TerraformAPIDotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {

        public ILogger<CommandController> _logger { get; set; }

        private readonly PowerShellService _powerShellService;

        public CommandController(ILogger<CommandController> logger, PowerShellService powerShellService)
        {
            _logger = logger;
            _powerShellService = powerShellService;
        }

        // POST api/<CommandController>
        [HttpPost]
        public IActionResult Post([FromBody] BodyRequestCommand bodyRequestCommand)
        {
            try
            {
                var response = _powerShellService.ExecuteCommand(bodyRequestCommand.script);
                var json = JsonSerializer.Serialize(response);

                if (response.error)
                {
                    return BadRequest(json);
                }

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        errorMessage = "Error: " + ex.Message,
                        error = true
                    }
                 );
            }
        }

    }
}


public class BodyRequestCommand
{
    public string script { get; set; } = string.Empty;
}