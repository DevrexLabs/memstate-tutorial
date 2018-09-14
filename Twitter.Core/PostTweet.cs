using System;
using Memstate;

namespace Twitter.Core
{
    public class PostTweet : Command<TwitterModel, int>
    {
        public readonly string UserName;
        public readonly string Message;
        public readonly DateTime PostedAt;

        public override int Execute(TwitterModel model)
        {
            var tweetId = model.PostTweet(UserName, Message, PostedAt);
            return tweetId;
        }
    }
}
