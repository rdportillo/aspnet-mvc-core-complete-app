using Dev.Business.Models.Validations.Document;
using FluentValidation;

namespace Dev.Business.Models.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 200).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            When(s => s.SupplierType == SupplierType.Person, () => {
                RuleFor(s => s.Document.Length).Equal(PersonDocumentValidation.CPF_LENGTH)
                    .WithMessage("The document field have {ComparisonValue} characters and it was supplied {PropertyValue}");

                RuleFor(s => PersonDocumentValidation.Validate(s.Document)).Equal(true)
                    .WithMessage("The provided document is invalid.");
            });

            When(s => s.SupplierType == SupplierType.Company, () => {
                RuleFor(s => s.Document.Length).Equal(CompanyDocumentValidation.CNPJ_LENGTH)
                    .WithMessage("The document field have {ComparisonValue} characters and it was supplied {PropertyValue}");

                RuleFor(s => CompanyDocumentValidation.Validate(s.Document)).Equal(true)
                    .WithMessage("The provided document is invalid.");
            });
        }
    }
}
