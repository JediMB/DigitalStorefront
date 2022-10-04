using Articles;

namespace Articles
{
    public class Mug : Article
    {
        public enum Type
        {
            Null = -1,
            Small,
            Medium,
            Large,
            Supersize
        }

        private Type _type;

        public Mug()
        {
            _type = Type.Null;
        }
    }
}
