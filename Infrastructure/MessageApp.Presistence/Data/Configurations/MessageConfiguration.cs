using MessageApp.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Presistence.Data.Configurations
{
    //public class MessageConfiguration: IEntityTypeConfiguration<MessagePrivate>
    //{
    //    public void Configure(EntityTypeBuilder<MessagePrivate> builder)
    //    {
    //        builder.ToTable("MessagesPrivate");

    //        builder.Property(mp => mp.Content).IsRequired().HasMaxLength(500);

    //        builder.HasOne(mp => mp.ToUser).WithMany(u => u.MessagesPrivate).HasForeignKey(mp => mp.ToUserId)
    //            .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(mp => mp.FromUser).WithMany(u => u.MessagesPrivate).HasForeignKey(mp => mp.FromUserId)
    //            .OnDelete(DeleteBehavior.Cascade);

    //    }
    //}

    //public class MessageConfiguration2 : IEntityTypeConfiguration<MessageGroup>
    //{
    //    public void Configure(EntityTypeBuilder<MessageGroup> builder)
    //    {
    //        builder.ToTable("MessagesGroup");

    //        builder.Property(mg => mg.Content).IsRequired().HasMaxLength(500);

    //        builder.HasOne(mg => mg.ToRoom).WithMany(r => r.MessagesGroup).HasForeignKey(mg => mg.ToRoomId)
    //            .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(mg => mg.FromUser).WithMany(u => u.MessagesGroup).HasForeignKey(mg => mg.FromUserId)
    //            .OnDelete(DeleteBehavior.Cascade);

    //    }
    //}
}
