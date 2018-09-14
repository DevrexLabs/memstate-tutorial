using System;

namespace Twitter.Core
{
    public class Tweet
    {
        public readonly int Id;
        public readonly string UserName;
        public readonly string Message;
        public readonly DateTime PostedAt;

        public Tweet(int id, string userName, string message, DateTime postedAt)
        {
            Id = id;
            UserName = userName;
            Message = message;
            PostedAt = postedAt;
        }
    }
}
