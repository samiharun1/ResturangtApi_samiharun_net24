using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ResturangtApi_samiharun_net24.Models;
using ResturangtApi_samiharun_net24.Models.Data;
using ResturangtApi_samiharun_net24.Models.Security;
using ResturangtApi_samiharun_net24.Models.Services;

// Den här filen startar hela ASP.NET Core-applikationen.
// Den konfigurerar:
// 1. Controllers (API-endpoints)
// 2. Swagger (för att testa API:t i webbläsaren)
// 3. CORS (så att React-appen kan prata med API:t)
// 4. Databasen med Entity Framework Core
// 5. JWT-autentisering (säker inloggning med token)
// 6. Seed-data (skapar admin, bord och meny om det inte finns)

namespace ResturangtApi_samiharun_net24
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant API", Version = "v1" });

                // JWT configuration for Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Write 'Bearer' followed by your token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Enable CORS for React
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Database
            builder.Services.AddDbContext<ResturangDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<BokningService>();

            // JWT Authentication
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
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Seed default data
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ResturangDbContext>();
                await db.Database.MigrateAsync();

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
                        new Meny { Name = "Cinnamon Roll", Price = 30, Description = "Freshly baked", IsPopular = true },
                        new Meny { Name = "Cappuccino", Price = 35, Description = "Foamy coffee", IsPopular = true }
                    );
                }

                await db.SaveChangesAsync();
            }

            app.Run();
        }
    }
}
