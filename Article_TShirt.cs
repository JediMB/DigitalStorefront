using Articles;

namespace Articles
{
    public class TShirt : Article
    {
        public enum Fabric
        {
            Null = -1,
            Cotton,
            Wool,
            Silk
        }

        private int _size;
        private Fabric _fabric;
        private float _averageReviewScore;

        public TShirt()
        {
            _size = -1;
            _fabric = Fabric.Null;
            _averageReviewScore = -1;
        }
    }
}
