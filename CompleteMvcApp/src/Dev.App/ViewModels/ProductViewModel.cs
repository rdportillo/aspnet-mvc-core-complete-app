﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dev.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(200, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(1000, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        public SupplierViewModel Supplier { get; set; }
    }
}
