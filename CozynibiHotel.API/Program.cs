using CozynibiHotel.Infrastructure.ServiceExtension;
using CozynibiHotel.Core.Helper;
using CozynibiHotel.Services.Interfaces;
using CozynibiHotel.Services.Services;
using CozynibiHotel.API.Hub;
using CozynibiHotel.API.Models;

using HUG.EmailServices.Models;
using HUG.EmailServices.Services;
using HUG.QRCodeServices.Services;
using HUG.SMS.Models;
using HUG.SMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddDIServices(builder.Configuration);


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();

//Rooms
builder.Services.AddScoped<IRoomCategoryService, RoomCategoryService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();

//Foods
builder.Services.AddScoped<IFoodCategoryService, FoodCategoryService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IFoodOrderService, FoodOrderService>();

//Tour Travel
builder.Services.AddScoped<ITourTravelService, TourTravelService>();
builder.Services.AddScoped<IInclusionService, InclusionService>();
builder.Services.AddScoped<IExclusionService, ExclusionService>();

//Service
builder.Services.AddScoped<IServiceService, ServiceService>();

//News
builder.Services.AddScoped<INewsCategoryService, NewsCategoryService>();
builder.Services.AddScoped<INewsService, NewsService>();

//PAGES
builder.Services.AddScoped<IPageService, PageService>();

//CUSTOMMER
builder.Services.AddScoped<ICustommerService, CustommerService>();

//ARTICLE
builder.Services.AddScoped<IArticleService, ArticleService>();

//GALLERY
builder.Services.AddScoped<IGalleryService, GalleryService>();

//INFORMATION
builder.Services.AddScoped<IInformationService, InformationService>();

//CONTACT
builder.Services.AddScoped<IContactService, ContactService>();

//BOOKING
builder.Services.AddScoped<IBookingService, BookingService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//SignalR
builder.Services.AddSignalR();

//Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//SMS
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddTransient<ISMSService, SMSService>();

builder.Services.AddControllers();

//Enable CORS
builder.Services.AddCors(p =>
    p.AddPolicy("HUG_LOCAL", build =>
    {
        build.WithOrigins("https://localhost:7034",
                          "http://localhost:5034",
                          "https://localhost:7289",
                          "http://localhost:5289",
                          "http://localhost:3000")
             .AllowAnyMethod()
             .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed((hosts) => true);
        //build.WithOrigins("*")
        //     .AllowAnyMethod()
        //     .AllowAnyHeader();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("HUG_LOCAL");

app.UseStaticFiles();// Add middleware for specify the static files in wwwroot

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/hub");
});

app.MapControllers();

app.Run();
