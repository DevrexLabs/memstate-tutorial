using Memstate;

namespace Twitter.Core
{
    public class Tweeted : Event
    {
        public readonly Tweet Tweet;

        public Tweeted(Tweet tweet)
        {
            Tweet = tweet;
        }
    }
}
