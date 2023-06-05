
using TicketProject.BusinessLayer.Category.Business;
using TicketProject.BusinessLayer.City.Business;
using TicketProject.BusinessLayer.Event.Business;
using TicketProject.BusinessLayer.TicketBusiness;
using TicketProject.BusinessLayer.TicketUserBusiness;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.EntityFramework.Category;
using TicketProject.DataLayer.EntityFramework.City;
using TicketProject.DataLayer.EntityFramework.CityFiles;
using TicketProject.DataLayer.EntityFramework.EventFiles;
using TicketProject.DataLayer.EntityFramework.TicketFiles;
using TicketProject.DataLayer.EntityFramework.TicketUserFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TPContext>();
builder.Services.AddIdentity<TicketUser, TicketUserRole>().AddEntityFrameworkStores<TPContext>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<ICityDal, EfCityDal>();
builder.Services.AddScoped<ICityService, CityManager>();

builder.Services.AddScoped<IEventDal, EfEventDal>();
builder.Services.AddScoped<IEventService, EventManager>();

builder.Services.AddScoped<ITicketDal, EfTicketDal>();
builder.Services.AddScoped<ITicketService, TicketManager>();

builder.Services.AddScoped<ITicketUserDal, EfTicketUserDal>();
builder.Services.AddScoped<ITicketUserService, TicketUserManager>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(option =>
{
    option.AddPolicy("TicketApiCors", opt =>
    {
        opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseCors("TicketApiCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
