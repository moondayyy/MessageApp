using AutoMapper;
using MessageApp.Application.ViewModels;
using MessageApp.Domain.Entities.Concrete;
using MessageApp.Infrastructure.SignalR.Hubs;
using MessageApp.Presistence.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MessageApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly MessageAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public GroupsController(MessageAppDbContext context, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        #region Aktif Oda Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<RoomViewModel> Get(string timestamp)
        {
            RoomViewModel roomViewModel = new();
            try
            {
                var room = await _context.Rooms.Where(r => r.CreatedDate == DateTime.Parse(timestamp) && r.IsActive == true).Include(r=>r.Users).FirstOrDefaultAsync();

                roomViewModel.AdminId = room.AdminId;
                roomViewModel.Avatar = room.Avatar;
                roomViewModel.RoomName = room.RoomName;
                roomViewModel.Timestamp = room.CreatedDate.ToString();
                roomViewModel.UserIds= room.Users.Select(u => u.Id).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Aktif Kullanıcı Getirme Hata: " + e.Message);
            }
            return roomViewModel;

        }
        #endregion

        #region Tüm Aktif Kullanıcıları Getirme
        [Route("[action]")]
        [HttpGet]
        public async Task<ICollection<UserViewModel>> GetList()
        {
            ICollection<UserViewModel> usersViewModel = null;
            try
            {
                var users = await _context.Users.Where(u => u.IsActive == true).ToListAsync();
                _mapper.Map(users, usersViewModel);
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




        //[Route("[action]")]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<RoomViewModel>>> Get()
        //{
        //    //var rooms = await _context.Rooms.ToListAsync();

        //    //var roomsViewModel = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(rooms);

        //    return Ok();
        //}

        //[Route("[action]")]
        //[HttpPost]
        //public async Task CreateAsync(RoomViewModel roomViewModel)
        //{
        //    //if (_context.Rooms.Any(r => r.RoomName == roomViewModel.RoomName))
        //    //    return BadRequest("Böyle bir oda zaten mevcut!");

        //    var user = _context.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == roomViewModel.PhoneNumber);



        //    var room = new Room()
        //    {
        //        Id = Guid.NewGuid(),
        //        RoomName = roomViewModel.RoomName,
        //        Admin = user,
        //        Avatar = roomViewModel.Avatar,

        //    };

        //    _context.Rooms.Add(room);
        //    await _hubContext.Clients.All.SendAsync("addChatRoom", new { id = room.Id, name = room.RoomName, avatar = room.Avatar });
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Hata: " + e.Message);
        //        throw;
        //    }



        //}
    }
}
