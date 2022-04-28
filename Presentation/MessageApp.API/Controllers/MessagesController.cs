using AutoMapper;
using MessageApp.Application.ViewModels;
using MessageApp.Domain.Entities.Concrete;
using MessageApp.Infrastructure.SignalR.Hubs;
using MessageApp.Presistence.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MessageApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(MessageAppDbContext context, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        #region Aktif Private Mesajları Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<ICollection<PrivateMessageViewModel>> GetPrivateList(string senderPhoneNumber, string reciverPhoneNumber)
        {
            ICollection<PrivateMessageViewModel> privateMessagesViewModel = null;
            try
            {
                var senderUser = _context.Users.Where(u => u.PhoneNumber == senderPhoneNumber && u.IsActive == true).FirstOrDefault();
                var senderUserId = senderUser.Id;
                var reciverUser = _context.Users.Where(u => u.PhoneNumber == reciverPhoneNumber && u.IsActive == true).FirstOrDefault();
                var reciverUserId = reciverUser.Id;
                var messages = _context.MessagesPrivate.Where(m => m.FromUserId == senderUserId && m.ToUserId == reciverUserId && m.IsActive == true).ToList();
                foreach (var message in messages)
                {
                    privateMessagesViewModel.Append(new()
                    {
                        Avatar = senderUser.Avatar,
                        Content = message.Content,
                        FromFullName = senderUser.FullName,
                        FromPhoneNumber = senderUser.PhoneNumber,
                        Timestamp = message.CreatedDate.ToString()
                    });
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Aktif Private Mesajları Getirme Hata: " + e.Message);
            }
            return privateMessagesViewModel;
        }
        #endregion

        #region Aktif Group Mesajları Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<ICollection<GroupMessageViewModel>> GetGroup(Guid groupId)
        {
            ICollection<GroupMessageViewModel> groupMessagesViewModel = null;
            try
            {
                var messages = await _context.MessagesGroup.Where(mg => mg.ToRoomId == groupId && mg.IsActive == true).Include(u => u.FromUser).ToListAsync();

                foreach (var message in messages)
                {
                    groupMessagesViewModel.Append(new GroupMessageViewModel()
                    {
                        Avatar = message.FromUser.Avatar,
                        Content = message.Content,
                        FromFullName = message.FromUser.FullName,
                        FromPhoneNumber = message.FromUser.PhoneNumber,
                        Timestamp = message.CreatedDate.ToString()
                    });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Aktif Group Mesajları Getirme Hata: " + e.Message);
            }
            return groupMessagesViewModel;
        }
        #endregion

        #region Private Mesaj Ekleme
        [Route("[action]")]
        [HttpPost]
        public async Task AddPrivate(PrivateMessageViewModel privateMessageViewModel)
        {
            var senderUser = await _context.Users.Where(u => u.PhoneNumber == privateMessageViewModel.FromPhoneNumber && u.IsActive == true).FirstOrDefaultAsync();
            var senderUserId = senderUser.Id;
            var reciverUser = await _context.Users.Where(u => u.PhoneNumber == privateMessageViewModel.ReciverPhoneNumber && u.IsActive == true).FirstOrDefaultAsync();
            var reciverUserId = reciverUser.Id;
            try
            {
                MessagePrivate messagePrivate = new()
                {
                    Id = Guid.NewGuid(),
                    Content = privateMessageViewModel.Content,
                    CreatedDate = DateTime.UtcNow,
                    FromUserId = senderUserId,
                    ToUserId = senderUserId,
                    FromUser = senderUser,
                    IsActive = true

                };

                await _context.MessagesPrivate.AddAsync(messagePrivate);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Private Mesaj Ekleme Hata: " + e.Message);
            }
        }
        #endregion

        #region Group Mesaj Ekleme
        [Route("[action]")]
        [HttpPost]
        public async Task AddGroup(GroupMessageViewModel groupMessageViewModel)
        {
            var senderUser = await _context.Users.Where(u => u.PhoneNumber == groupMessageViewModel.FromPhoneNumber && u.IsActive == true).FirstOrDefaultAsync();
            var senderUserId = senderUser.Id;
            var toRoom = await _context.Rooms.Where(r => r.Id == groupMessageViewModel.ToRoomId && r.IsActive == true).FirstOrDefaultAsync();
            var toRoomId = toRoom.Id;
            try
            {
                MessageGroup messageGroup = new()
                {
                    Content = groupMessageViewModel.Content,
                    CreatedDate = DateTime.UtcNow,
                    FromUser = senderUser,
                    FromUserId = senderUserId,
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    ToRoom = toRoom,
                    ToRoomId = toRoomId

                };

                await _context.MessagesGroup.AddAsync(messageGroup);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Group Mesaj Ekleme Hata: " + e.Message);
            }
        }
        #endregion

        #region Private Mesaj Soft Silme
        [Route("[action]")]
        [HttpPut]
        public async Task RemoveSoftPrivate(string timestamp)
        {
            try
            {
                var message = _context.MessagesPrivate.FirstOrDefault(mp => mp.CreatedDate == DateTime.Parse(timestamp));
                message.IsActive = false;
                _context.MessagesPrivate.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Private Mesaj Soft Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Group Mesaj Soft Silme
        [Route("[action]")]
        [HttpPut]
        public async Task RemoveSoftGroup(string timestamp)
        {
            try
            {
                var message = _context.MessagesGroup.FirstOrDefault(mg => mg.CreatedDate == DateTime.Parse(timestamp));
                message.IsActive = false;
                _context.MessagesGroup.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Group Mesaj Soft Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Private Mesaj Hard Silme
        [Route("[action]")]
        [HttpDelete]
        public async Task RemoveHardPrivate(string timestamp)
        {
            try
            {
                var message = _context.MessagesPrivate.FirstOrDefault(mp => mp.CreatedDate == DateTime.Parse(timestamp));
                _context.MessagesPrivate.Remove(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Private Mesaj Hard Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Group Mesaj Hard Silme
        [Route("[action]")]
        [HttpDelete]
        public async Task RemoveHardGroup(string timestamp)
        {
            try
            {
                var message = _context.MessagesGroup.FirstOrDefault(mg => mg.CreatedDate == DateTime.Parse(timestamp));
                _context.MessagesGroup.Remove(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Group Mesaj Hard Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Private Mesaj Güncelleme
        [Route("[action]")]
        [HttpPut]
        public async Task UpdatePrivate(PrivateMessageViewModel privateMessageViewModel)
        {
            try
            {
                var message = _context.MessagesPrivate.FirstOrDefault(mp => mp.CreatedDate == DateTime.Parse(privateMessageViewModel.Timestamp) && mp.IsActive == true);
                message.ModifiedDate = DateTime.UtcNow;
                message.Content = privateMessageViewModel.Content;
                _context.MessagesPrivate.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Private Mesaj Güncelleme Hata: " + e.Message);
            }

        }
        #endregion

        #region Group Mesaj Güncelleme
        [Route("[action]")]
        [HttpPut]
        public async Task UpdateGroup(GroupMessageViewModel groupMessageViewModel)
        {
            try
            {
                var message = _context.MessagesGroup.FirstOrDefault(mg => mg.CreatedDate == DateTime.Parse(groupMessageViewModel.Timestamp) && mg.IsActive == true);
                message.ModifiedDate = DateTime.UtcNow;
                message.Content = groupMessageViewModel.Content;
                _context.MessagesGroup.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Group Mesaj Güncelleme Hata: " + e.Message);
            }

        }
        #endregion

    }
}
