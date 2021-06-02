using System;

namespace SqlQueryBuilder.Domain
{
    public class Car
    {
        public int Id { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public double Price { get; set; }

        public DateTime SaleDate { get; set; }
    }
}