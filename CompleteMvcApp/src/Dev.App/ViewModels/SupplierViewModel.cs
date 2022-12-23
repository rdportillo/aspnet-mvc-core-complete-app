using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dev.App.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(200, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(14, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }

        [DisplayName("Type")]
        public int SupplierType { get; set; }

        public AddressViewModel Address { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
