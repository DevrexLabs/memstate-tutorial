using System;
using System.Linq;
using Memstate;

namespace Twitter.Core
{
    public class FolloweeNames : Query<TwitterModel, string[]>
    {
        public readonly string UserName;

        public FolloweeNames(string userName)
        {
            UserName = userName;
        }

        public override string[] Execute(TwitterModel model)
        {
            return model.UsersByName.TryGetValue(UserName, out var user)
                ? user
                    .Followees
                    .Select(f => f.Name)
                    .ToArray()
                : Array.Empty<string>();
        }
    }
}
