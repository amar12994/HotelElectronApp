using ElectronNET.WebApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronNET.WebApp.Domain.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("menuItem");
            builder.Property(en => en.Id).HasColumnName("id");
            builder.Property(en => en.MenuCode).HasColumnName("menu_code");
            builder.Property(en => en.Description).HasColumnName("description");
            builder.Property(en => en.CompanyCode).HasColumnName("company_code");
            builder.Property(en => en.StatusCode).HasColumnName("status_code");
            builder.Property(en => en.EffectiveDate).HasColumnName("effective_date");
            builder.Property(en => en.CreatedDate).HasColumnName("created_date");
            builder.Property(en => en.CreatedBy).HasColumnName("created_by");
            builder.Property(en => en.LastModifiedDate).HasColumnName("last_modified_date");
            builder.Property(en => en.LastModifiedBy).HasColumnName("last_modified_by");
        }
    }
}