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
using HUG.EmailServices.Services;
using HUG.QRCodeServices.Services;

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
            //ACCOUNT & TOKEN
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRefeshTokenRepository, RefeshTokenRepository>();

            //LANGUAGE
            services.AddScoped<ILanguageRepository, LanguageRepository>();

            //ROOM
            services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            services.AddScoped<IRoomImageRepository, RoomImageRepository>();
            services.AddScoped<IRoomEquipmentRepository, RoomEquipmentRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomGalleryRepository, RoomGalleryRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();

            //FOOD
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IFoodOrderRepository, FoodOrderRepository>();
            services.AddScoped<IFoodOrderDetailsRepository, FoodOrderDetailsRepository>();

            //TRAVEL
            services.AddScoped<ITourTravelRepository, TourTravelRepository>();
            services.AddScoped<ITourExclusionRepository, TourExclusionRepository>();
            services.AddScoped<ITourInclusionRepository, TourInclusionRepository>();
            services.AddScoped<ITourPriceRepository, TourPriceRepository>();
            services.AddScoped<ITourScheduleRepository, TourScheduleRepository>();
            services.AddScoped<ITourGalleryRepository, TourGalleryRepository>();
            services.AddScoped<IExclusionRepository, ExclusionRepository>();
            services.AddScoped<IInclusionRepository, InclusionRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IGalleryCategoryRepository, GalleryCategoryRepository>();

            //SERVICE
            services.AddScoped<IServiceRepository, ServiceRepository>();


            //NEWS
            services.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();

            //PAGES
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageBannerRepository, PageBannerRepository>();

            //CUSTOMMER
            services.AddScoped<ICustommerRepository, CustommerRepository>();

            //ARTICLE
            services.AddScoped<IArticleRepository, ArticleRepository>();

            //INFORMATION
            services.AddScoped<IInformationRepository, InformationRepository>();

            //CONTACT
            services.AddScoped<IContactRepository, ContactRepository>();

            //BOOKING
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddTransient<IEmailService, EmailService>();

            //Authenization

            //Khai baos Appsetting map voweis cais  gif, cho pheps copy
            services.Configure<AppSetting>(configuration.GetSection("AppSettings"));
            //QR
            services.AddScoped<IQRCodeService, QRCodeService>();

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
