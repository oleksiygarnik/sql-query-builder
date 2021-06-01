using Microsoft.AspNetCore.Mvc;
using SqlQueryBuilder.Infrastructure;

namespace SqlQueryBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly BaseDbContext _dbContext;

        public HealthCheckController(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public string Get()
        {
            return "Health check";
        }
    }
}
