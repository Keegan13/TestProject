using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<ProjectDeveloper> BindEntity { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
           // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Project>(ConfigureProjects);
            mb.Entity<Developer>(ConfigureDevelopers);
            mb.Entity<ProjectDeveloper>(ConfigureBindEntity);
        }

        protected void ConfigureProjects(EntityTypeBuilder<Project> proj)
        {
            proj.ToTable("projects");
            proj.HasKey(x => x.Id);
            proj.HasAlternateKey(x => x.Name);
            proj.Property(x => x.Id).HasColumnName("project_id");
            proj.Property(x => x.Name).HasColumnName("project_name").IsRequired().HasMaxLength(150);
            proj.Property(x => x.Description).HasColumnName("project_description").HasMaxLength(450);
            proj.Property(x => x.StartDate).HasColumnName("project_start_date");
            proj.Property(x => x.EndDate).HasColumnName("project_end_date");
        }
        protected void ConfigureDevelopers(EntityTypeBuilder<Developer> dev)
        {
            dev.ToTable("developers");
            dev.HasKey(x => x.Id);
            dev.HasAlternateKey(x => x.Nickname);
            dev.Property(x => x.Id).HasColumnName("developer_id");
            dev.Property(x => x.FullName).HasColumnName("developer_fullname").IsRequired().HasMaxLength(150);
            dev.Property(x => x.Nickname).HasColumnName("developer_nickname").HasMaxLength(100);
        }
        protected void ConfigureBindEntity(EntityTypeBuilder<ProjectDeveloper> bind)
        {
            bind.ToTable("project_developer");
            bind.HasKey(x => new { x.ProjectId, x.DeveloperId });
            bind.Property(x => x.ProjectId).HasColumnName("project_id");
            bind.Property(x => x.DeveloperId).HasColumnName("developer_id");
            bind.HasOne(x => x.Project).WithMany(x => x.ProjectDevelopers).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            bind.HasOne(x => x.Developer).WithMany(x => x.ProjectDevelopers).HasForeignKey(x => x.DeveloperId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
