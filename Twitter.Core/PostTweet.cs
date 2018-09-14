using System;
using Memstate;

namespace Twitter.Core
{
    public class PostTweet : Command<TwitterModel, int>
    {
        public readonly string UserName;
        public readonly string Message;
        public readonly DateTime PostedAt;

        public PostTweet(string user, string message, DateTime postedAt)
        {
            UserName = user;
            Message = message;
            PostedAt = postedAt;
        }

        public override int Execute(TwitterModel model)
        {
            var tweetId = model.PostTweet(UserName, Message, PostedAt);
            return tweetId;
        }
    }
}
