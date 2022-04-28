using AutoMapper;
using MessageApp.Application.Twilio;
using MessageApp.Application.ViewModels;
using MessageApp.Domain.Entities.Concrete;
using MessageApp.Infrastructure.SignalR.Hubs;
using MessageApp.Infrastructure.TwilioSMS;
using MessageApp.Presistence.Data.Contexts;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.EntityFrameworkCore;

namespace MessageApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MessageAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ISenderSMS _senderSMS;
        private readonly IHubContext<ChatHub> _hubContext;

        public UsersController(MessageAppDbContext context, IMapper mapper, IMemoryCache cache, ISenderSMS senderSMS, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _senderSMS = senderSMS;
            _hubContext = hubContext;
        }
        #region Aktif Kullanıcı Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<UserViewModel> Get(string phoneNumber)
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                var user = await _context.Users.Where(u => u.PhoneNumber == phoneNumber && u.IsActive == true).FirstOrDefaultAsync();
                _mapper.Map(user, userViewModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Aktif Kullanıcı Getirme Hata: " + e.Message);
            }
            return userViewModel;

        }
        #endregion

        #region Tüm Aktif Kullanıcıları Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<ICollection<UserViewModel>> GetList()
        {
            ICollection<UserViewModel> usersViewModel = null;
            UserViewModel userViewModel = null;
            try
            {
                var users = await _context.Users.Where(u => u.IsActive == true).ToListAsync();
                foreach (var user in users)
                {
                    _mapper.Map(user, userViewModel);
                    usersViewModel.Append(userViewModel);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Tüm Aktif Kullanıcıları Getirme Hata: " + e.Message);
            }


            return usersViewModel;
        }
        #endregion

        #region Kullanıcı Ekleme
        [Route("[action]")]
        [HttpPost]
        public async Task Add(UserViewModel usersViewModel)
        {
            try
            {
                User user = new()
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ConnectionId = ""
                };
                _mapper.Map(usersViewModel, user);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Ekleme Hata: " + e.Message);
            }

        }
        #endregion

        #region Kullanıcı Soft Silme
        [Route("[action]")]
        [HttpPut]
        public async Task RemoveSoft(string phoneNumber)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                user.IsActive = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Soft Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Kullanıcı Hard Silme
        [Route("[action]")]
        [HttpDelete]
        public async Task RemoveHard(string phoneNumber)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Hard Silme Hata: " + e.Message);
            }

        }
        #endregion

        #region Kullanıcı Güncelleme
        [Route("[action]")]
        [HttpPut]
        public async Task Update(UserViewModel usersViewModel)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == usersViewModel.PhoneNumber && u.IsActive == true);
                user.ModifiedDate = DateTime.UtcNow;
                _mapper.Map(usersViewModel, user);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Güncelleme Hata: " + e.Message);
            }

        }
        #endregion

        #region Kullanıcı DB 'de Olup Olmadığını Kontrol Etme
        [Route("[action]")]
        [HttpGet]
        public async Task<bool> IsRegistered(string phoneNumber)
        {
            bool result = false;
            try
            {
                if (await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber) != null)
                {
                    result = true;
                }
                else
                {
                    result = false;

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı DB 'de Olup Olmadığını Kontrol Etme Hata: " + e.Message);
            }
            return result;
        }
        #endregion

        #region Kullanıcı Telefonuna Doğrulama SMS Kodu Gönderme
        [Route("[action]")]
        [HttpPost]
        public async Task SendSMS(string phoneNumber)
        {
            try
            {
                await _senderSMS.SendSMSAsync(phoneNumber);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Telefonuna Doğrulama SMS Kodu Gönderme Hata: " + e.Message);
            }

        }
        #endregion

        #region Kullanıcı Telefonuna Gelen SMS Doğrulaması Kontrol
        [Route("[action]")]
        [HttpPost]
        public string VerifyCode(string phoneNumber, string verifyCode)
        {
            string result = null;
            try
            {
                result = _senderSMS.VerifyCodeCheck(phoneNumber, verifyCode);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Kullanıcı Telefonuna Gelen SMS Doğrulaması Kontrol Hata: " + e.Message);
            }
            return result;
        }
        #endregion

        //[Route("[action]")]
        //[HttpPost]
        //public async Task<ActionResult<UserViewModel>> LoginAsync(string phoneNumber)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        //    UserViewModel userViewModel = new()
        //    {
        //        Avatar = user.Avatar,
        //        PhoneNumber = user.PhoneNumber,
        //        ConnectionId = user.ConnectionId,
        //        Description = user.Description,
        //        FullName = user.FullName,
        //    };
        //    //await _hubContext.Clients.All.SendAsync("addChatRoom", new { id = room.Id, name = room.RoomName });
        //    return Ok(userViewModel);
        //}

    }
}
