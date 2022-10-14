using Articles;

namespace Articles
{
    public class Mug : Article
    {
        public enum Types
        {
            Null = -1,
            Small,
            Medium,
            Large,
            Supersize
        }

        private readonly Types _type;

        public Types Type { get => _type; }

        public Mug(string print, decimal price, Types type, float avgScore)
        {
            _print = print;
            _price = price;
            _type = type;
            _averageReviewScore = avgScore;
        }

        public override string ToString()
        {
            return $"{_print} {_type} {_averageReviewScore:F1} {_price:C2}";
        }

        public string ToString(int printWidth, int priceWidth, int typeWidth, int scoreWidth, int padding = 0, int extraPadding = 0)
        {
            return " " + _print.PadRight(printWidth + padding) +
                $"{_type}".PadRight(typeWidth + padding) +
                $"{_averageReviewScore:F1}".PadLeft(padding + extraPadding + scoreWidth) +
                $"{_price:C2}".PadLeft(padding + priceWidth) +
                " ░"; // These two characters at the end are a hack for the scrollbar. :(
        }
    }
}
