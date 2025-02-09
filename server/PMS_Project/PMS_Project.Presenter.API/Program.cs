using Microsoft.OpenApi.Models;
using PMS_Project.Presenter.API.Middlewares;
using PMS_Project.Presenter.API;
using PMS_Project.Presenter.API.Utils;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//lines 21 and 22 were originally placed here

// Define the directory path for keys
string keyDirectory = Path.Combine(builder.Environment.ContentRootPath, "./");

// Instantiate GenerateKeyPairs
// This will generate and save the key pair upon application startup
var keyGenerator = new GenerateKeyPairs(keyDirectory);

// Register Project Dependencies
builder.Services.AddProjectDependencies(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project Management System API",
        Version = "v1"
    });

    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        // Scheme = JwtBearerDefaults.AuthenticationScheme
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Apply the security scheme globally
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });


});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowOrigin",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)  // Allow any origin in development
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
});
app.MapGet("/swagger-ui/SwaggerDark.css", async (CancellationToken cancellationToken) =>
{
    var css = await File.ReadAllBytesAsync("SwaggerDark.css", cancellationToken);
    return Results.File(css, "text/css");
}).ExcludeFromDescription();
//}

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseCors("AllowOrigin"); // Must be after UseRouting and before UseAuthorization

// app.UseMiddleware<XMiddleware>(); // remove this later
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();