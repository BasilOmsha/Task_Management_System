namespace PMS_Project.InfrastructureConfigurations
{
    /**
    * Enitity Config class
    * Contains the configuration for the x entity 
    * * This class is intended to configure the X model for Entity Framework Core.
        * *   - It will define the database schema details for the x example(SystemUser) table, such as:
        * *   - Table name mapping
        * *   - Primary keys and default values
        * *   - Property configurations (e.g., required fields, max lengths)
        * *   - Indexes and unique constraints
        * *   - Relationships with other entities (if any)
    * After creating all the config files, from the solution root (PMS_Project) run:
    * ? `dotnet ef migrations add InitialCreate --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API`
    * ? `dotnet ef database update --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API`
    */
    public class EntityXConfig //: IEntityTypeConfiguration<EntityX>
    {
        
        // public void Configure(EntityTypeBuilder<EntityX> builder)
        // {
        //     // Table name
        //     builder.ToTable("SystemUser");

        //     // Primary Key
        //     builder.HasKey(e => e.Id);

        //     // Configure Id to use gen_random_uuid()
        //     builder.Property(e => e.Id)
        //         .HasDefaultValueSql("gen_random_uuid()");

        //     // Firstname
        //     builder.Property(e => e.Firstname)
        //         .IsRequired()
        //         .HasMaxLength(30);

        //     // Lastname
        //     builder.Property(e => e.Lastname)
        //         .IsRequired()
        //         .HasMaxLength(30);

        //     // Username
        //     builder.Property(e => e.Username)
        //         .IsRequired()
        //         .HasMaxLength(60);

        //     builder.HasIndex(e => e.Username)
        //         .IsUnique();

        //     // Email
        //     builder.Property(e => e.Email)
        //         .IsRequired()
        //         .HasMaxLength(100);

        //     builder.HasIndex(e => e.Email)
        //         .IsUnique();

        //     // Password
        //     builder.Property(e => e.Password)
        //         .IsRequired()
        //         .HasMaxLength(100);

        //     // CreatedAt with default value NOW()
        //     builder.Property(e => e.CreatedAt)
        //         .IsRequired()
        //         .HasDefaultValueSql("NOW()");

        //     // UpdatedAt with default value NOW()
        //     builder.Property(e => e.UpdatedAt)
        //         .IsRequired()
        //         .HasDefaultValueSql("NOW()");
        // }
    }
}