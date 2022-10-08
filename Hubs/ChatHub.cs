using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Models;
using SignalRSwaggerGen.Attributes;

namespace RealTimeChat.Hubs

{
    public interface IChatHub {
        Task ListUsers(string[] users);


        Task RequestedPublicKey(Interaction interaction) ;    

        Task ReceivedPublicKey(PublicKeyInteraction publicKeyInteraction) ;
         
        Task ReceivedMessage(MessageInteraction messageInteraction) ;      
    }
    [SignalRHub]
    public class chatHub: Hub<IChatHub> {
        public static Dictionary<string, string> Users = new Dictionary<string,string>();

        public async Task Init(string user) { 
            if (Users.ContainsKey(user))
            {
                Users.Remove(user);
            }
            Users.Add(user, Context.ConnectionId);
            await Clients.All.ListUsers(Users.Keys.ToArray());
        }

        public async Task StartConversation(string toUser) {
            if (Users.ContainsValue(Context.ConnectionId))
            {
                var fromUser = Users.First(user => user.Value == Context.ConnectionId).Key;
                await Clients.Client(Users[toUser]).RequestedPublicKey(new Interaction(fromUser, toUser));
                await Clients.Client(Users[fromUser]).RequestedPublicKey(new Interaction(toUser, fromUser));
            }
        }

        public async Task SendPublicKey(string toUser, string publicKey)
        {
            if (Users.ContainsValue(Context.ConnectionId)) {
            var fromUser = Users.First(user => user.Value == Context.ConnectionId).Key;
            await Clients.Client(Users[toUser]).ReceivedPublicKey(new PublicKeyInteraction(fromUser, toUser, publicKey));
            }
        }

        public async Task SendMessage(string toUser, List<byte> message)
        {
            if (Users.ContainsValue(Context.ConnectionId)) 
            {
            var fromUser = Users.First(user => user.Value == Context.ConnectionId).Key;
            await Clients.Client(Users[toUser]).ReceivedMessage( new MessageInteraction(fromUser, toUser, message));
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)

        {
            if (Users.ContainsValue(Context.ConnectionId))
            {
                Users.Remove(Users.First(user => user.Value == Context.ConnectionId).Key);
                await Clients.All.ListUsers(Users.Keys.ToArray());
             }
            await base.OnDisconnectedAsync(exception);
        }
    }

}