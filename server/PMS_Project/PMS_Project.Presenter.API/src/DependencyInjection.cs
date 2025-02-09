using Microsoft.EntityFrameworkCore;
using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.Utils;
using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Infrastructure.Database;
using PMS_Project.Infrastructure.Repositories;
using PMS_Project.Infrastructure.Service;
using PMS_Project.Presenter.API.Utils;
using PMS_Project.Presenter.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PMS_Project.Application.Services;
using Microsoft.AspNetCore.Authorization;
using PMS_Project.Application.Common.Constants;
using PMS_Project.Application.Interfaces.Repositories;
using PMS_Project.Application.Interfaces.Services;
using PMS_Project.Domain.src.Abstractions.Repositories;


namespace PMS_Project.Presenter.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            // Register Infrastructure Services
            RegisterDBContextAndRepos(services, configuration);

            // Register Application Services
            RegisterServices(services);

            // Register Middleware
            RegisterMiddleware(services);

            // Register Libraries
            RegisterLibraries(services);

            // Register Utilities
            RegisterUtilities(services);

            // Register Authentication
            RegisterAuthentication(services, configuration);

            // Register Authorization
            RegisterAuthorizationPolicies(services);


            return services;
        }

        private static void RegisterDBContextAndRepos(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<PostgreSQLDbContext>(options =>
            {

                #region Config PostgreSQL Database
                // var connectionString = configuration.GetConnectionString("localpostgresql"); // local development
                var connectionString = configuration.GetConnectionString("DefaultConnection"); // production
                try
                {
                    options.UseNpgsql(connectionString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in PostgreSQL Connection: " + ex.Message);
                }
            });
            #endregion

            // Register Repositories
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProjectBoardUserRepository, ProjectBoardUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IWorkspaceUserRepository, WorkspaceUserRepository>();
            services.AddScoped<IProjectBoardRepository, ProjectBoardRepository>();
            services.AddScoped<IPriorityRepository, PriorityRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITaskCardRepository, TaskCardRepository>();
            services.AddScoped<ITaskCardActivityRepository, TaskCardActivityRepository>();
            services.AddScoped<ITaskCardChecklistRepository, TaskCardChecklistRepository>();
            services.AddScoped<ITaskCardChecklistItemRepository, TaskCardChecklistItemRepository>();
            services.AddScoped<IProjectBoardLabelRepository, ProjectBoardLabelRepository>();
            services.AddScoped<IAppListRepository, AppListRepository>();

        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAppListService, AppListService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaswdService, PaswdService>();
            //services.AddScoped<IProjectBoardRepository, ProjectBoardRepository>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IWorkspaceUserService, WorkspaceUserService>();
            services.AddScoped<IProjectBoardService, ProjectBoardService>();
            services.AddScoped<IProjectBoardUserService, ProjectBoardUserService>();
            services.AddSingleton<IJwtRsaKeysService, JwtRsaKeysService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAppAuthService, AppAuthService>();
            services.AddScoped<IPriorityService, PriorityService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ITaskCardService, TaskCardService>();
            services.AddScoped<ITaskCardActivityService, TaskCardActivityService>();
            services.AddScoped<ITaskCardChecklistService, TaskCardChecklistService>();
            services.AddScoped<ITaskCardChecklistItemService, TaskCardChecklistItemService>();
            services.AddScoped<IProjectBoardLabelService, ProjectBoardLabelService>();

        }

        private static void RegisterMiddleware(IServiceCollection services)
        {

        }

        private static void RegisterLibraries(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
        }

        private static void RegisterUtilities(IServiceCollection services)
        {
            services.AddSingleton<GenerateKeyPairs>(provider =>
            {
                // Specify the directory where keys will be saved
                string keyDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Keys");

                // Ensure the directory exists
                if (!Directory.Exists(keyDirectory))
                {
                    Directory.CreateDirectory(keyDirectory);
                }

                return new GenerateKeyPairs(keyDirectory);
            });
        }

        private static void RegisterAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve RSA keys from RSAKeysService
            var rsaKeysService = services.BuildServiceProvider().GetService<IJwtRsaKeysService>();

            // Get RSA keys
            var validationKey = new RsaSecurityKey(rsaKeysService!.ValidationKey); // Public Key

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = validationKey,
                    ClockSkew = TimeSpan.Zero
                };

                // check if the token is expired. Help to revoke the access token
                opts.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        var userRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var user = await userRepo.GetByIdAsync(Guid.Parse(userId));

                        if (user == null || user.TokenRevoked)
                        {
                            context.Fail("Token has been revoked");
                        }
                    }
                };
            });
        }

        public static IServiceCollection AddAuthorizationPolicies(IServiceCollection services)
        {
            // services.AddScoped<IAuthorizationHandler, WorkspaceRoleHandler>();

            // Define Authorization Policies
            services.AddAuthorization(options =>
            {
                // Workspace Policies
                // options.AddPolicy("WorkspaceOwnerPolicy", policy =>
                //     policy.Requirements.Add(new WorkspaceOwnerRequirement()));

                // options.AddPolicy("WorkspaceAdminPolicy", policy =>
                //     policy.Requirements.Add(new WorkspaceRoleRequirement(Roles.Admin)));

                // // Project Policies
                // options.AddPolicy("ProjectOwnerPolicy", policy =>
                //     policy.Requirements.Add(new ProjectRoleRequirement(Roles.Owner)));

                // options.AddPolicy("ProjectAdminPolicy", policy =>
                //     policy.Requirements.Add(new ProjectRoleRequirement(Roles.Admin)));

                // options.AddPolicy("ProjectContributorPolicy", policy =>
                //     policy.Requirements.Add(new ProjectRoleRequirement(Roles.Contributor)));
            });

            return services;
        }

        private static IServiceCollection RegisterAuthorizationPolicies(IServiceCollection services)
        {
            AddAuthorizationPolicies(services);
            return services;
        }

    }
}
