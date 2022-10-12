namespace Articles
{
    public abstract class Article
    {
        protected string _print;
        protected decimal _price;
        protected float _averageReviewScore;

        public string Print { get => _print; }
        public decimal Price { get => _price; }
        public float AverageReviewScore { get => _averageReviewScore; }

        public Article()
        {
            _print = string.Empty;
            _price = -1;
            _averageReviewScore = -1;
        }
    }
}
