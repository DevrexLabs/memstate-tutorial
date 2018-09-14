using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Memstate;
using Memstate.Configuration;
using Twitter.Core;
using Xunit;

namespace Twitter.Test
{
    public class IntegrationTests
    {
        Engine<TwitterModel> _engine;

        public IntegrationTests()
        {
            Task.Run( async () =>
            {
                var cfg = Config.Current;
                cfg.UseInMemoryFileSystem();

                var settings = cfg.GetSettings<EngineSettings>();
                settings.WithRandomSuffixAppendedToStreamName();

                _engine = await Engine.Start<TwitterModel>();

                var cmd1 = new PostTweet("bart", "This is the worst day of my life", DateTime.Now);
                await _engine.Execute(cmd1);

                var cmd2 = new PostTweet("homer", "@bart the worst day yet", DateTime.Now);
                await _engine.Execute(cmd2);

                var cmd3 = new PostTweet("bart", ". @homer Eat my shorts", DateTime.Now);
                await _engine.Execute(cmd3);
            }).Wait();
        }

        [Fact]
        public async Task Tweets_are_added_to_users_sequence_of_tweets()
        {
            var cmd = new UsersTweets("bart");
            var bartsTweets = await _engine.Execute(cmd);

            Assert.Equal(2, bartsTweets.Length);
            Assert.Equal(3, bartsTweets[1].Id);
        }

        [Fact]
        public async Task Tweets_can_be_retrieved_and_have_increasing_ids()
        {
            var cmd = new AllTweets(take: 5);
            var tweets = await _engine.Execute(cmd);

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
