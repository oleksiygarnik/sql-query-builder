using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlQueryBuilder.Domain;
using SqlQueryBuilder.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlQueryBuilder.Controllers
{
    public class CarDto
    {
        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public double Price { get; set; }

        public DateTime SaleDate { get; set; }
    }

    public class VirtualTableFilters
    {
        public string Column { get; set; }

        public string Value { get; set; }
    }

    public class AddVirtualTableCommand
    {
        public string TableName { get; set; }

        public VirtualTableFilters[] Filters { get; set; }

        public string[] Filials { get; set; }
    }

    [ApiController]
    [Route("api")]
    public class CarController : ControllerBase
    {
        private readonly BaseDbContext _dbContext;

        public CarController(BaseDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("virtual-tables")]
        public async Task<IEnumerable<VirtualTable>> GetAllVirtualTables()
        {
            return await _dbContext.VirtualTables.ToListAsync();
        }

        [HttpPost("virtual-tables/add")]
        public async Task<int> AddVirtualTable(AddVirtualTableCommand command)
        {
            var virtualTable = new VirtualTable
            {
                Name = command.TableName
            };
            await _dbContext.VirtualTables.AddAsync(virtualTable);
            await _dbContext.SaveChangesAsync();

            foreach (var filial in command.Filials)
            {
                if (!command.Filters.Any())
                {
                    var fragment = new Fragment
                    {
                        Table = virtualTable,
                        Query = "SELECT * FROM [Showroom].[dbo].[Car]"
                    };

                    fragment.Query = filial == command.Filials.First()
                        ? fragment.Query
                        : @$"SELECT * FROM OPENROWSET('SQLOLEDB', 'Server={filial};UID=sa;PWD=sa;', '{fragment.Query}')";

                    fragment.Table = virtualTable;

                    await _dbContext.Fragments.AddAsync(fragment);

                    continue;
                }

                foreach (var member in command.Filters)
                {
                    var memberValue = member.Value;

                    memberValue = !int.TryParse(memberValue, out _)
                        ? filial == command.Filials.First() ? $"'{memberValue}'" : $"''{memberValue}''"
                        : memberValue;

                    var fragment = new Fragment
                    {
                        Table = virtualTable,
                        Query = $"SELECT * FROM [Showroom].[dbo].[Car] WHERE {member.Column} = {memberValue}"
                    };

                    fragment.Query = filial == command.Filials.First()
                        ? fragment.Query
                        : @$"SELECT * FROM OPENROWSET('SQLOLEDB', 'Server={filial};UID=sa;PWD=sa;', '{fragment.Query}')";

                    fragment.Table = virtualTable;

                    await _dbContext.Fragments.AddAsync(fragment);
                }
            }

            await _dbContext.SaveChangesAsync();

            return virtualTable.Id;
        }

        [HttpDelete("virtual-tables/{Id}")]
        public async Task RemoveVirtualTable(int id)
        {
            var table = await _dbContext.VirtualTables.SingleAsync(t => t.Id == id);
            var fragments = await _dbContext.Fragments.Where(f => f.Table.Id == table.Id).ToListAsync();
  
            _dbContext.RemoveRange(fragments);
            await _dbContext.SaveChangesAsync();

            _dbContext.VirtualTables.Remove(table);
            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("virtual-tables/{tableId}/cars")]
        public async Task<IEnumerable<Car>> GetCars(int tableId)
        {
            var fragments = await _dbContext.Fragments.Where(f => f.Table.Id == tableId).ToListAsync();

            var query = fragments.Select(f => f.Query).Aggregate((a, b) => $"{a} UNION ALL {b}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(query);
            Console.ForegroundColor = ConsoleColor.Gray;

            return _dbContext.Cars.FromSqlRaw(query);
        }

        [HttpPost("cars/add")]
        public async Task<int> AddCar(CarDto dto)
        {
            var car = new Car
            {
                BrandName = dto.BrandName,
                ModelName = dto.ModelName,
                Price = dto.Price,
                SaleDate = dto.SaleDate
            };

            await _dbContext.Cars.AddAsync(car);

            await _dbContext.SaveChangesAsync();

            return car.Id;
        }
    }
}