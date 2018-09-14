using Memstate;

namespace Twitter.Core
{
    public class UsersTweets : Query<TwitterModel, Tweet[]>
    {
        public readonly string UserName;
        public readonly int Skip;
        public readonly int Take;

        public UsersTweets(string userName, int skip, int take)
        {
            UserName = userName;
            Skip = skip;
            Take = take;
        }

        public override Tweet[] Execute(TwitterModel model)
        {
            return model.GetUserTweets(UserName, Skip, Take);
        }
    }
}
