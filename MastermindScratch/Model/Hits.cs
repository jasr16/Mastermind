namespace MastermindScratch.Model
{
    public class Hits
    {
        public int FullHits { get; set; }
        public int ColorHits { get; set; }

        public Hits(int full, int color)
        {
            FullHits = full;
            ColorHits = color;
        }

    }
}
