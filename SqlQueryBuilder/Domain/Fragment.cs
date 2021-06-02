namespace SqlQueryBuilder.Domain
{
    public class Fragment
    {
        public int Id { get; set; }

        public string Query { get; set; }

        public VirtualTable Table { get; set; }
    }
}