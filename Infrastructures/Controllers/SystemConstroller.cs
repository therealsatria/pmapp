using Microsoft.AspNetCore.Mvc;
using Infrastructures.Services;
using System.Threading.Tasks;

namespace pmapp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public SystemController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet("dbstatus")]
        public async Task<IActionResult> GetDatabaseStatus()
        {
            var isConnected = await _databaseService.CheckConnectionAsync();
            return Ok(new { isConnected });
        }
    }
}