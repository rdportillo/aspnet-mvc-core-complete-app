namespace Dev.Business.Models.Validations.Document
{
    public class PersonDocumentValidation
    {
        public const int CPF_LENGTH = 11;

        public static bool Validate(string personDocument)
        {
            var cpfDocument = DocumentValidationUtils.NumbersOnly(personDocument);

            if (!ValidLength(cpfDocument)) return false;

            return !RepeatedDigits(cpfDocument) && ValidDigits(cpfDocument);
        }

        private static bool ValidLength(string document)
        {
            return document.Length == CPF_LENGTH;
        }

        private static bool RepeatedDigits(string document)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };

            return invalidNumbers.Contains(document);
        }

        private static bool ValidDigits(string document)
        {
            var number = document.Substring(0, CPF_LENGTH - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersFromTo(2, 11)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == document.Substring(CPF_LENGTH - 2, 2);
        }
    }
}
