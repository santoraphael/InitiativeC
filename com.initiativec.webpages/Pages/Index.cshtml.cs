using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public NumberModel NumberData { get; set; }
        public void OnGet()
        {
            long number = 2510483431;
            NumberData = ProcessNumber(number);
        }

        private NumberModel ProcessNumber(long number)
        {
            var numberString = number.ToString("N0").Replace(".", ","); // Formata o número com vírgulas
            var digits = numberString.Replace(",", "").Select(digit => int.Parse(digit.ToString())).ToList();

            var commaPositions = numberString.Select((c, i) => new { c, i })
                                             .Where(x => x.c == ',')
                                             .Select(x => x.i - x.i / 4)
                                             .ToList();

            var numberModel = new NumberModel();

            foreach (var digit in digits)
            {
                numberModel.Cards.Add(new CardModel
                {
                    UpperValue = digit,
                    LowerValue = 0, // Ajuste conforme necessário
                    FirstFlipValue = 0, // Ajuste conforme necessário
                    SecondFlipValue = digit
                });
            }

            numberModel.CommaPositions = commaPositions;

            return numberModel;
        }
    }

    public class NumberModel
    {
        public List<CardModel> Cards { get; set; } = new List<CardModel>();
        public List<int> CommaPositions { get; set; } = new List<int>();
    }

    public class CardModel
    {
        public int UpperValue { get; set; }
        public int LowerValue { get; set; }
        public int FirstFlipValue { get; set; }
        public int SecondFlipValue { get; set; }
    }
}
