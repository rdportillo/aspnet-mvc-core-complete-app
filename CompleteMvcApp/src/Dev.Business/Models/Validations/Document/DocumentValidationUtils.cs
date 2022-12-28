namespace Dev.Business.Models.Validations.Document
{
    public class DocumentValidationUtils
    {
        public static string NumbersOnly(string document)
        {
            var numberOnly = "";

            foreach (var number in document)
            {
                if(char.IsDigit(number))
                {
                    numberOnly += number;
                }
            }
            return numberOnly.Trim();
        }
    }
}
