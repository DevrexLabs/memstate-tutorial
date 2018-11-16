using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Twitter.Web.Hubs
{
    public class TwitterHub : Hub
    {
        readonly EventRelay _eventRelay;

        public TwitterHub(EventRelay eventRelay)
        {
            _eventRelay = eventRelay;
        }

        public Task Subscribe(string userName)
        {
            var id = Context.ConnectionId;
            return _eventRelay.Connect(userName, id);
        }
    }
}
