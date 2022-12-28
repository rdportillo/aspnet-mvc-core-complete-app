namespace Dev.Business.Models.Validations.Document
{
    public class VerifyingDigit
    {
        private const int MODULE = 11;

        private string _number;
        private readonly List<int> _multipliers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _substitutions = new Dictionary<int, string>();
        private readonly bool _moduleComplement = true;

        public VerifyingDigit(string number)
        {
            _number = number;
        }

        public VerifyingDigit WithMultipliersFromTo(int firstMultiplier, int lastMultiplier)
        {
            _multipliers.Clear();
            for (var i = firstMultiplier; i <= lastMultiplier; i++)
                _multipliers.Add(i);

            return this;
        }

        public VerifyingDigit Replacing(string substitute, params int[] digits)
        {
            foreach (var d in digits)
            {
                _substitutions[d] = substitute;
            }
            return this;
        }

        public void AddDigit(string digit)
        {
            _number = string.Concat(_number, digit);
        }

        public string CalculateDigit()
        {
            return (_number.Length <= 0) ? "" : GetDigitSum();
        }

        private string GetDigitSum()
        {
            var sum = 0;
            for (int i = _number.Length - 1, m = 0; i >= 0; i--)
            {
                var result = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
                sum += result;

                if (++m >= _multipliers.Count) m = 0;
            }

            var mod = (sum % MODULE);
            var resultado = _moduleComplement ? MODULE - mod : mod;

            return _substitutions.ContainsKey(resultado) ? _substitutions[resultado] : resultado.ToString();
        }
    }
}
