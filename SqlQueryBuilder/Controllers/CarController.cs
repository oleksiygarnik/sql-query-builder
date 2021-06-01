using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlQueryBuilder.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SqlQueryBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly BaseDbContext _dbContext;

        public CarController(BaseDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var kek = _dbContext.Cinema.FromSqlRaw(@"SELECT * FROM OPENROWSET('SQLOLEDB',
'Server=LESHALAPTOP\MSSQLSERVER_TEMP;UID=sa;PWD=sa;', 'SELECT * FROM CinemaDwh.dbo.[Cinema]')");

            return "kok";
        }
    }
}
