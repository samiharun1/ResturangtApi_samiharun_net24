
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;
using ResturangtApi_samiharun_net24.Models.Data;
using ResturangtApi_samiharun_net24.Models.Security;
using ResturangtApi_samiharun_net24.Models.Services;

namespace ResturangtApi_samiharun_net24
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ResturangDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<BokningService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Seed: skapa admin och några bord/meny om tomt
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ResturangDbContext>();
                await db.Database.MigrateAsync(); // <-- async

                if (!db.Admins.Any())
                {
                    var (hash, salt) = PasswordHelper.HashPassword("Admin123!");
                    db.Admins.Add(new Admin { Username = "admin", PasswordHash = hash, PasswordSalt = salt });
                }

                if (!db.Bord.Any())
                {
                    db.Bord.AddRange(
                        new Bord { BordNummer = 1, Kapacitet = 2 },
                        new Bord { BordNummer = 2, Kapacitet = 4 },
                        new Bord { BordNummer = 3, Kapacitet = 6 }
                    );
                }

                if (!db.Meny.Any())
                {
                    db.Meny.AddRange(
                        new Meny { Name = "Kanelbulle", Price = 30, Description = "Nygräddad", IsPopular = true },
                        new Meny { Name = "Cappuccino", Price = 35, Description = "Skummig kaffe", IsPopular = true }
                    );
                }

                await db.SaveChangesAsync(); // <-- async
            }

            app.Run();
        }
    }
}