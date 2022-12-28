namespace Dev.Business.Models.Validations.Document
{
    public class CompanyDocumentValidation
    {
        public const int CNPJ_LENGTH = 14;

        public static bool Validate(string companyDocument)
        {
            var cnpjDocument = DocumentValidationUtils.NumbersOnly(companyDocument);

            if (!ValidLength(cnpjDocument)) return false;

            return !RepeatedDigits(cnpjDocument) && ValidDigits(cnpjDocument);
        }

        private static bool ValidLength(string document)
        {
            return document.Length == CNPJ_LENGTH;
        }

        private static bool RepeatedDigits(string document)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };

            return invalidNumbers.Contains(document);
        }

        private static bool ValidDigits(string document)
        {
            var number = document.Substring(0, CNPJ_LENGTH - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersFromTo(2, 9)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == document.Substring(CNPJ_LENGTH - 2, 2);
        }
    }
}
