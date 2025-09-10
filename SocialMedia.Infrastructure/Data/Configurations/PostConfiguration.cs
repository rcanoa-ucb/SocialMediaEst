using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Publicacion");

            builder.ToTable("Post");

            builder.Property(e => e.Date).HasColumnType("datetime");
            builder.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            builder.Property(e => e.Imagen)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_User");
        }
    }
}
