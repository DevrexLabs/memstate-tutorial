using Memstate;

namespace Twitter.Core
{
    public class Follow : Command<TwitterModel>
    {
        public readonly string Follower;
        public readonly string Followee;

        public Follow(string follower, string followee)
        {
            Follower = follower;
            Followee = followee;
        }

        public override void Execute(TwitterModel model)
        {
            model.Follow(Follower, Followee);
        }
    }
}
