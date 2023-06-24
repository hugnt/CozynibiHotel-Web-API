using CozynibiHotel.Infrastructure.ServiceExtension;
using CozynibiHotel.Core.Helper;
using CozynibiHotel.Services.Interfaces;
using CozynibiHotel.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddDIServices(builder.Configuration);
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IRoomCategoryService, RoomCategoryService>();
builder.Services.AddScoped<IRoomService, RoomService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Enable CORS
builder.Services.AddCors(p =>
    p.AddPolicy("HUG_LOCAL", build =>
    {
        build.WithOrigins("https://localhost:7034",
                          "http://localhost:5034",
                          "https://localhost:7289",
                          "http://localhost:5289")
             .AllowAnyMethod()
             .AllowAnyHeader();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
