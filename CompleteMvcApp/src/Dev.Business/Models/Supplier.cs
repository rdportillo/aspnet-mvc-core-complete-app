namespace Dev.Business.Models
{
    public class Supplier : Entity
    {
        public string Name { get; set; }

        public string Document { get; set; }

        public SupplierType SupplierType { get; set; }

        public Address Address { get; set; }

        public bool Active { get; set; }

        // Entity Framework Relation
        public IEnumerable<Product> Products { get; set; }
    }
}
