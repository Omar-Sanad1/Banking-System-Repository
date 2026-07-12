using BankingManagementSystemBigProject.CreateTokenService;
using BankingManagementSystemBigProject.ExceptionMiddlewares;
using BankingManagementSystemBigProject.Helper;
using BankingManagementSystemBigProject.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Context;
using Repository.Repository;
using Service.Interfaces;
using Service.Services;
using System.Text;

namespace BankingManagementSystemBigProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<BankingSystemDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BankingConnection")));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));

            builder.Services.AddScoped(typeof(IAccountService), typeof(AccountService));
            builder.Services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
            builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
            builder.Services.AddScoped(typeof(ITransactionService), typeof(TransactionService));
            builder.Services.AddScoped(typeof(IBranchService), typeof(BranchService));


            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"]
                };
            });

            var app = builder.Build();

            using(var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var dbContext = service.GetRequiredService<BankingSystemDbContext>();
                    await dbContext.Database.MigrateAsync();
                    await BankingDataSeeding.SeedAsync(dbContext);
                }
                catch(Exception ex)
                {
                    var logger = LoggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Error happen when migration.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
