using FluentValidation;

namespace Dev.Business.Models.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 200).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 1000).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("The {PropertyName} field must be greather than {ComparisonValue}");
        }
    }
}
