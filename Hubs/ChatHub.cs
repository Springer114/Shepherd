using Microsoft.AspNetCore.SignalR;
using Shepherd.Models;
using System.Threading.Tasks;

namespace Shepherd.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}