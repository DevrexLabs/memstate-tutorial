using Memstate;

namespace Twitter.Core
{
    public class Unfollow : Command<TwitterModel>
    {
        public readonly string Follower;
        public readonly string Followee;

        public Unfollow(string follower, string followee)
        {
            Follower = follower;
            Followee = followee;
        }

        public override void Execute(TwitterModel model)
        {
            model.Unfollow(Follower, Followee);
        }
    }
}
