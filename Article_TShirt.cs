using Articles;

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
    }
}
