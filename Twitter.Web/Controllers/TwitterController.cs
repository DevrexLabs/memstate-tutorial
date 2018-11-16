using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Memstate;
using Microsoft.AspNetCore.Mvc;
using Twitter.Core;

namespace Twitter.Web.Controllers
{
    public class TwitterController : Controller
    {
        private readonly Client<TwitterModel> _client;

        public TwitterController(Client<TwitterModel> client)
        {
            _client = client;
        }

        public async Task<IActionResult> AllTweets(int skip = 0, int take = 20)
        {
            var tweets = await _client.Execute(new AllTweets(skip, take));
            return Json(tweets);
        }

        public async Task<IActionResult> UserTweets(string user, int skip = 0, int take = 20)
        {
            var tweets = await _client.Execute(new UsersTweets(user, skip, take));
            return Json(tweets);
        }

        [HttpPost]
        public async Task<IActionResult> Tweet(string user, string message)
        {
            if (Regex.IsMatch(message, "^(un)?follow @\\S+$", RegexOptions.IgnoreCase))
            {
                string followee = message.Substring(message.IndexOf("@") + 1);
                if (message.StartsWith("un", StringComparison.InvariantCultureIgnoreCase))
                {
                    await _client.Execute(new Unfollow(user, followee));
                }
                else await _client.Execute(new Follow(user, followee));
            }
            var cmd = new PostTweet(user, message, DateTime.Now);
            var tweetId = await _client.Execute(cmd);
            return Json(new { id = tweetId });
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string user, string followee)
        {
            var cmd = new Follow(user, followee);
            await _client.Execute(cmd);
            return Ok();
        }
    }
}
