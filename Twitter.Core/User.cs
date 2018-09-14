using System.Collections.Generic;

namespace Twitter.Core
{
    public class User
    {
        public readonly string Name;

        /// <summary>
        /// This users tweets
        /// </summary>
        public List<Tweet> Tweets { get; private set; }

        /// <summary>
        /// Users that follow this user
        /// </summary>
        /// <value>The followers.</value>
        public List<User> Followers { get; private set; }

        /// <summary>
        /// Users this user follows
        /// </summary>
        public List<User> Followees { get; private set; }

        public User(string name)
        {
            Name = name;
            Tweets = new List<Tweet>();
            Followees = new List<User>();
            Followers = new List<User>();
        }

    }
}
