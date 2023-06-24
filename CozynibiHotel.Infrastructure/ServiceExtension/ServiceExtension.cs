using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Infrastructure.Data;
using CozynibiHotel.Infrastructure.Repository;
using CozynibiHotel.Core.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CozynibiHotel.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRefeshTokenRepository, RefeshTokenRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            services.AddScoped<IRoomImageRepository, RoomImageRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();


            //Authenization

            //Khai baos Appsetting map voweis cais  gif, cho pheps copy
            services.Configure<AppSetting>(configuration.GetSection("AppSettings")); 

            var secretKey = configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    //ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
