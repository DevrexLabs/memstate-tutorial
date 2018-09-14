using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitter.Core
{
    public class TwitterModel
    {
        private int nextTweetId = 1;

        /// <summary>
        /// Users indexed by user name
        /// </summary>
        public IDictionary<string, User> UsersByName { get; private set; }

        /// <summary>
        /// Tweets ordered id.
        /// </summary>
        public IDictionary<int, Tweet> AllTweets { get; private set; }

        public TwitterModel()
        {
            AllTweets = new SortedDictionary<int, Tweet>();
            UsersByName = new SortedDictionary<string, User>();
        }

        public int PostTweet(string userName, string message, DateTime when)
        {
            EnsureUserExists(userName);
            var tweet = new Tweet(nextTweetId++, userName, message, when);
            AllTweets.Add(tweet.Id, tweet);
            var user = UsersByName[userName];
            user.Tweets.Add(tweet);
            return tweet.Id;
        }

        public void Follow(string followerName, string followeeName)
        {
            if (UsersByName.TryGetValue(followerName, out var follower))
            {
                if (UsersByName.TryGetValue(followeeName, out var followee))
                {
                    follower.Followees.Add(followee);
                    followee.Followers.Add(follower);
                    return;
                }
                throw new ArgumentException("No such user: " + followeeName);
            }
            throw new ArgumentException("No such user: " + followerName);
        }

        public Tweet[] GetAllTweets(int skip = 0, int take = 20)
        {
            return AllTweets
                .Values
                .Skip(skip)
                .Take(take)
                .ToArray();
        }

        public Tweet[] GetUserTweets(string userName, int skip = 0, int take = 20)
        {
            if (UsersByName.TryGetValue(userName, out var user))
            {
                return user.Tweets.Skip(skip).Take(take).ToArray();
            }
            throw new ArgumentException("No such user: " + userName);
        }

        private void EnsureUserExists(string userName)
        {
            if (!UsersByName.ContainsKey(userName))
            {
                var user = new User(userName);
                UsersByName.Add(userName, user);
            }
        }
    }
}
