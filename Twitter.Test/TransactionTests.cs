using System;
using Twitter.Core;
using Xunit;

namespace Twitter.Test
{

    public class TransactionTests
    {
        readonly TwitterModel _twitter;

        public TransactionTests()
        {
            _twitter = new TwitterModel();

            var cmd1 = new PostTweet("bart", "This is the worst day of my life", DateTime.Now);
            cmd1.Execute(_twitter);

            var cmd2 = new PostTweet("homer", "@bart the worst day yet", DateTime.Now);
            cmd2.Execute(_twitter);

            var cmd3 = new PostTweet("bart", ". @homer Eat my shorts", DateTime.Now);
            cmd3.Execute(_twitter);
        }

        [Fact]
        public void Tweets_are_added_to_users_sequence_of_tweets()
        {
            var cmd = new UsersTweets("bart");
            var bartsTweets = cmd.Execute(_twitter);

            Assert.Equal(2, bartsTweets.Length);
            Assert.Equal(3, bartsTweets[1].Id);
        }

        [Fact]
        public void Tweets_can_be_retrieved_and_have_increasing_ids()
        {
            var cmd = new AllTweets(take:5);
            var tweets = cmd.Execute(_twitter);

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
