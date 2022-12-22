namespace Dev.Business.Models
{
    public class Address : Entity
    {
        public Guid SupplierId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        // Entity Framework Relation
        public Supplier Supplier { get; set; }
    }
}
