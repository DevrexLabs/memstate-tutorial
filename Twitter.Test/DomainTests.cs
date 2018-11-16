using System;
using System.Linq;
using Twitter.Core;
using Xunit;

namespace Twitter.Test
{
    public class DomainTests
    {
        readonly TwitterModel _twitter;

        public DomainTests()
        {
            _twitter = new TwitterModel();
            var tweet1 = _twitter.PostTweet("bart", "This is the worst day of my life", DateTime.Now);
            var tweet2 = _twitter.PostTweet("homer", "@bart the worst day yet", DateTime.Now);
            var tweet3 = _twitter.PostTweet("bart", ". @homer Eat my shorts", DateTime.Now);
        }

#pragma warning disable RECS0063 // Warns when a culture-aware 'StartsWith' call is used by default.

        [Fact]
        public void Tweets_are_added_to_users_sequence_of_tweets()
        {
            var bartsTweets = _twitter.GetUserTweets("bart");
            Assert.Equal(2, bartsTweets.Length);
            Assert.Equal(3, bartsTweets[1].Id);
        }

        [Fact]
        public void Tweets_can_be_retrieved_and_have_increasing_ids()
        {
            var tweets = _twitter.GetAllTweets(0, 5);
            Assert.Equal(3, tweets.Length);
            Assert.Equal(1, tweets[0].Id);
            Assert.Equal("bart", tweets[0].UserName);
            Assert.Equal("This is the worst day of my life", tweets[0].Message);

            Assert.Equal(2, tweets[1].Id);
            Assert.Equal("homer", tweets[1].UserName);

            Assert.Equal(3, tweets[2].Id);
            Assert.Equal("bart", tweets[0].UserName);
        }
    }
}
