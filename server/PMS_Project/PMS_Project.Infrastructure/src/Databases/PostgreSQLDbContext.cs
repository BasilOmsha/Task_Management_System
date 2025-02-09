using Microsoft.EntityFrameworkCore;
using PMS_Project.Domain.Models;

namespace PMS_Project.Infrastructure.Database
{
    public class PostgreSQLDbContext : DbContext
    {
        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<Workspace_User> Workspace_User { get; set; }
        public DbSet<ProjectBoard> ProjectBoard { get; set; }
        public DbSet<ProjectBoard_User> ProjectBoard_User { get; set; }
        public DbSet<AppList> AppList { get; set; }
        public DbSet<TaskCard> TaskCard { get; set; }
        public DbSet<TaskCard_User> TaskCard_User { get; set; }
        public DbSet<TaskCardActivity> TaskCardActivity { get; set; }
        public DbSet<ProjectBoardLabel> ProjectBoardLabel { get; set; }
        public DbSet<TaskCard_ProjectBoardLabel> TaskCard_ProjectBoardLabel { get; set; }
        public DbSet<TaskCardChecklist> TaskCardChecklist { get; set; }
        public DbSet<TaskCardChecklistItem> TaskCardChecklistItem { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Priority> Priority { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>()
                .ToTable("AppRole") // Role is a reserved keyword in PostgreSQL
                .HasData(
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Owner",
                        Description = "Owner of the workspace with full permissions."
                    },
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        Description = "Administrator with elevated permissions within the workspace."
                    },
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Contributor",
                        Description = "Contributor with limited permissions within the workspace."
                    }
                );

            modelBuilder.Entity<Status>()
                .HasData(
                    new Status
                    {
                        Id = Guid.NewGuid(),
                        Name = "New"
                    },
                    new Status
                    {
                        Id = Guid.NewGuid(),
                        Name = "In Progress"
                    },
                    new Status
                    {
                        Id = Guid.NewGuid(),
                        Name = "Done"
                    }
                );

            // Add Priority seeding
            modelBuilder.Entity<Priority>()
                .HasData(
                    new Priority
                    {
                        Id = Guid.NewGuid(),
                        Name = "Low"
                    },
                    new Priority
                    {
                        Id = Guid.NewGuid(),
                        Name = "Medium"
                    },
                    new Priority
                    {
                        Id = Guid.NewGuid(),
                        Name = "High"
                    }
                );

            // Add Users
            modelBuilder.Entity<User>()
                .HasData(
                    // Repeat the pattern for remaining users up to 30
                    Enumerable.Range(0, 30).Select(i => new User
                    {
                        Id = Guid.NewGuid(),
                        Firstname = $"User{i}",
                        Lastname = "Test",
                        Email = $"user{i}@email.com",
                        Username = $"user{i}",
                        Password = "Qwerty123!",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }).ToArray()
                );


            modelBuilder.Entity<User>()
                .ToTable("AppUser"); // User is a reserved keyword in PostgreSQL

            // Define composite primary key for Workspace_User
            modelBuilder.Entity<Workspace_User>()
                .ToTable("Workspace_AppUser")
                    .HasKey(wu => new { wu.UserId, wu.WorkspaceId });

            modelBuilder.Entity<Workspace_User>()
                .HasOne(wu => wu.User)
                .WithMany(u => u.Workspace_Users)
                .HasForeignKey(wu => wu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define composite primary key for ProjectBoard_User
            modelBuilder.Entity<ProjectBoard_User>()
                .ToTable("ProjectBoard_AppUser")
                    .HasKey(pbu => new { pbu.UserId, pbu.ProjectBoardId });

            modelBuilder.Entity<ProjectBoard_User>()
                .HasOne(pbu => pbu.User)
                .WithMany(u => u.ProjectBoard_Users)
                .HasForeignKey(pbu => pbu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define composite primary key for TaskCard_User
            modelBuilder.Entity<TaskCard_User>()
                .ToTable("TaskCard_AppUser")
                .HasKey(tcu => new { tcu.UserId, tcu.TaskCardId });

            modelBuilder.Entity<TaskCard_User>()
                .HasOne(tcu => tcu.User)
                .WithMany(u => u.TaskCard_Users)
                .HasForeignKey(tcu => tcu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define composite primary key for TaskCard_ProjectBoardLabel
            modelBuilder.Entity<TaskCard_ProjectBoardLabel>()
                .HasKey(tpbl => new { tpbl.TaskCardId, tpbl.ProjectBoardLabelId });

            // Configure SystemUser entity
            // modelBuilder.ApplyConfiguration(new SystemUserConfig()); //or:
            // Automatically apply all configurations in the assembly
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreSQLDbContext).Assembly);

        }
    }
}
