using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Memstate;
using Microsoft.AspNetCore.SignalR;
using Twitter.Core;

namespace Twitter.Web.Hubs
{
    public class EventRelay
    {
        readonly IHubContext<TwitterHub> _hub;

        /// <summary>
        /// Memstate client connection, the event source
        /// </summary>
        readonly Client<TwitterModel> _client;

        /// <summary>
        /// Connected users and names of who they are following
        /// </summary>
        readonly Dictionary<string, ISet<String>> _connectedUsersAndFollowers;

        public EventRelay(IHubContext<TwitterHub> hub, Client<TwitterModel> client)
        {
            _hub = hub;
            _client = client;
            _connectedUsersAndFollowers = new Dictionary<string, ISet<string>>();
            HandleTweeted().GetAwaiter().GetResult();
        }

        /// <summary>
        /// A web socket client has connected, get the names of the users the
        /// connecting user follows
        /// </summary>
        public async Task Connect(string userName, string id)
        {
            var names = await _client.Execute(new FolloweeNames(userName));
            var key = userName + "@" + id;
            _connectedUsersAndFollowers[key] = new HashSet<string>(names);
        }

        /// <summary>
        /// Relay the Tweeted event to connected users if they
        /// are either mentioned or following the user posting the tweet
        /// </summary>
        /// <returns>The subscribe.</returns>
        public Task HandleTweeted()
        {
            return _client.Subscribe<Tweeted>(async t =>
            {
                foreach (var key in _connectedUsersAndFollowers.Keys)
                {
                    var userNameAndConnectionId = key.Split("@");
                    var userName = userNameAndConnectionId[0];
                    var connectionId = userNameAndConnectionId[1];
                    var data = new
                    {
                        user = t.Tweet.UserName,
                        message = t.Tweet.Message,
                        postedAt = t.Tweet.PostedAt,
                        isMention = IsMention(userName, t.Tweet.Message),
                        isTimeline = _connectedUsersAndFollowers[key].Contains(t.Tweet.UserName)
                    };

                    var firehose = true;

                    if (data.isMention || data.isTimeline || firehose)
                    {
                        await _hub
                            .Clients.Client(connectionId)
                            .SendAsync("tweet", data);
                    }
                }
            });
        }

        /// <summary>
        /// True if a message mentions the specific user
        /// </summary>
        /// <returns><c>true</c>, if mention was ised, <c>false</c> otherwise.</returns>
        /// <param name="userName">User name without the @</param>
        /// <param name="message">The message to check</param>
        private bool IsMention(string userName, string message)
        {
            return Regex.IsMatch(message, "@" + userName + "\\b", RegexOptions.IgnoreCase);
        }
    }

}
