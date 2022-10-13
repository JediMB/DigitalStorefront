using Articles;
using System.Reflection;

namespace Articles
{
    public class TShirt : Article
    {
        public enum Fabrics
        {
            Null = -1,
            Cotton,
            Wool,
            Silk
        }

        public enum Sizes
        {
            Null = -1,
            XS, S, M, L, XL, XXL
        }

        private readonly Sizes _size;
        private readonly Fabrics _fabric;

        public Sizes Size { get => _size; }
        public Fabrics Fabric { get => _fabric; }

        public TShirt()
        {
            _size = Sizes.Null;
            _fabric = Fabrics.Null;
        }

        public TShirt(string print, decimal price, Sizes size, Fabrics fabric, float avgScore)
        {
            _print = print;
            _price = price;
            _size = size;
            _fabric = fabric;
            _averageReviewScore = avgScore;
        }

        public override string ToString()
        {
            return $"{_print} {_fabric} {_size} {_averageReviewScore:F1} {_price:C2}";
        }

        public string ToString(int printWidth, int priceWidth, int sizeWidth, int fabricWidth, int scoreWidth, int padding = 0, int extraPadding = 0)
        {
            return _print.PadRight(printWidth + padding) +
                $"{_fabric}".PadRight(fabricWidth + padding) +
                $"{_size}".PadRight(sizeWidth + padding) +
                $"{_averageReviewScore:F1}".PadLeft(padding + extraPadding + scoreWidth) +
                $"{_price:C2}".PadLeft(padding + priceWidth);
        }
    }
}
