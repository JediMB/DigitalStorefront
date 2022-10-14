namespace Articles
{
    public abstract class Article : IComparable<Article>
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

        int IComparable<Article>.CompareTo(Article? article)    // Thursday afternoon (Oct 13), I had to figure out how to code *generalized* comparison methods
        {                                                       // Friday afternoon, I'd thoroughly explained it to two others. Hooray.
            if (article != null)
            {
                int scoreComparison = _averageReviewScore.CompareTo(article._averageReviewScore);

                if (scoreComparison == 0)
                    return String.Compare(this._print, article._print);

                return scoreComparison;
            }
            
            throw new ArgumentNullException(nameof(article));
        }

        public static IComparer<Article> CompareByAvgScoreDescending() => new CompareAvgScoreDescending();

        private class CompareAvgScoreDescending : IComparer<Article>
        {
            int IComparer<Article>.Compare(Article? a, Article? b)
            {
                if (a != null && b != null)
                {
                    int scoreComparison = b._averageReviewScore.CompareTo(a._averageReviewScore);

                    if (scoreComparison == 0)
                        return String.Compare(a._print, b._print);

                    return scoreComparison;
                }

                if (a == null)
                    throw new ArgumentNullException(nameof(a));

                throw new ArgumentNullException(nameof(b));
            }
        }
    }
}
