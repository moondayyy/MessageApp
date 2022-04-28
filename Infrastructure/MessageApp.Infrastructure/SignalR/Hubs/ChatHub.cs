using AutoMapper;
using MessageApp.Application.ViewModels;
using MessageApp.Domain.Entities.Concrete;
using MessageApp.Presistence.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Infrastructure.SignalR.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {
        //public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        //private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MessageAppDbContext _context;
        private readonly IMapper _mapper;

        public ChatHub(MessageAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string getConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task Connect(string phoneNumber)
        {
            try
            {
                var user = _context.Users.Where(u => u.PhoneNumber == phoneNumber).First();
                user.ConnectionId = Context.ConnectionId;
                await _context.SaveChangesAsync();
                await Clients.Caller.SendAsync("getProfileInfo", user);
            }
            catch (Exception ex)
            {

                await Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                //var user = _context.ApplicationUsers.Where(u => u.PhoneNumber == _httpContextAccessor.HttpContext.Request.Cookies["phoneNumber"]).First();
                //user.ConnectionId = "";
                //_context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivate(string senderPhoneNumber, string reciverPhoneNumber, string message,string roomName="")
        {
            var sender = _context.Users.Where(u => u.PhoneNumber == senderPhoneNumber).First();
            var reciverUser = _context.Users.Where(u => u.PhoneNumber == reciverPhoneNumber).FirstOrDefault();
            var reciverConnectionId = reciverUser.ConnectionId;
            if (!string.IsNullOrEmpty(message.Trim()))
            {
                var messageViewModel = new PrivateMessageViewModel()
                {
                    Content = message,
                    FromFullName = sender.FullName,
                    Avatar = sender.Avatar,
                    //Room = "",
                    Timestamp = DateTime.Now.ToLongTimeString(),
                    FromPhoneNumber = sender.PhoneNumber
                };
                try
                {
                    //var room = _context.Rooms.FirstOrDefault(r => r.RoomName == roomName);
                    //if (room == null)
                    //{
                    //    Room room1 = new Room()
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        RoomName = "",
                    //        //Admin = null,
                    //        Avatar = "",
                            
                    //    };

                    //    _context.Messages.Add(new MessagePrivate()
                    //    {
                    //        Content = messageViewModel.Content,
                    //        FromUser = sender,
                    //        Id = Guid.NewGuid(),
                    //        //ToRoom = room1,
                    //        //ToUser=reciverUser

                    //    });
                    //}
                    //else
                    //{
                    //    _context.Messages.Add(new MessagePrivate()
                    //    {
                    //        Content = messageViewModel.Content,
                    //        FromUser = sender,
                    //        Id = Guid.NewGuid(),
                    //        //ToRoom = room,
                            

                    //    });
                    //}
                    
                    //await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    Debug.WriteLine("Hata: " + e.Message);
                }
                
                //await Clients.Client(reciverConnectionId).SendAsync("newMessage", messageViewModel);
                //await Clients.Caller.SendAsync("newMessage", messageViewModel);
                await Clients.Clients(new List<string>{ reciverConnectionId,sender.ConnectionId }).SendAsync("newMessage", messageViewModel);


            }

        }

        //public IEnumerable<UserViewModel> GetUsers(string roomName)
        //{
        //    return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        //}

        //public async Task Leave(string roomName)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        //}

        //public async Task Join(string roomName)
        //{
        //    try
        //    {
        //        var user = _Connections.Where(u => u.PhoneNumber == IdentityName).FirstOrDefault();
        //        if (user != null && user.CurrentRoom != roomName)
        //        {
        //            // Remove user from others list
        //            if (!string.IsNullOrEmpty(user.CurrentRoom))
        //                await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

        //            // Join to new chat room
        //            await Leave(user.CurrentRoom);
        //            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        //            user.CurrentRoom = roomName;

        //            // Tell others to update their list of users
        //            await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
        //    }
        //}

        //private string IdentityName
        //{
        //    get { return Context.User.Identity.Name; }
        //}

        //private string GetDevice()
        //{
        //    var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
        //    if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
        //        return device;

        //    return "Web";
        //}
    }
}
