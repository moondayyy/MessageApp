using MessageApp.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MessageApp.Presistence.Data.Contexts
{
    public class MessageAppDbContext : DbContext
    {
        public MessageAppDbContext(DbContextOptions<MessageAppDbContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<MessagePrivate> MessagesPrivate { get; set; }
        public DbSet<MessageGroup> MessagesGroup { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserRoom> UsersRooms { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var datas = ChangeTracker.Entries<Base>();
        //    foreach (var data in datas)
        //    {
        //        var _ = data.State switch
        //        {
        //            EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
        //            EntityState.Modified => data.Entity.ModifiedDate = DateTime.UtcNow
        //        };
        //    }
        //    return await base.SaveChangesAsync(cancellationToken);
        //}
    }
}
