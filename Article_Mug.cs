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

        public Mug()
        {
            _type = Types.Null;
        }

        public Mug(string print, decimal price, Types type, float avgScore)
        {
            _print = print;
            _price = price;
            _type = type;
            _averageReviewScore = avgScore;
        }
    }
}
