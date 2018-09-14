﻿using Memstate;

namespace Twitter.Core
{
    public class AllTweets : Query<TwitterModel, Tweet[]>
    {
        public readonly int Skip;
        public readonly int Take;

        public AllTweets(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public override Tweet[] Execute(TwitterModel model)
        {
            return model.GetAllTweets(Skip, Take);
        }
    }
}
