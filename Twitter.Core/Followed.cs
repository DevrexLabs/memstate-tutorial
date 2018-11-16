using Memstate;

namespace Twitter.Core
{

    public class Followed : Event
    {
        public readonly string Follower;
        public readonly string Followee;

        public Followed(string follower, string followee)
        {
            Follower = follower;
            Followee = followee;
        }
    }
}
