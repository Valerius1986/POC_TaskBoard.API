using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using POC_TaskBoard.API.Context;
using POC_TaskBoard.API.Repos;
using POC_TaskBoard.API.Repos.Interface;
using POC_TaskBoard.API.Service;
using POC_TaskBoard.API.Service.Interfaces;
using POC_TaskBoard.API.SignalR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<ITaskBoardService, TaskBoardService>();
builder.Services.AddScoped<IStatusOfTaskRepository, StatusOfTaskRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        }));

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", builder =>
//    {
//        builder.WithOrigins("http://localhost:5173")
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});

var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(_logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<TaskBoardHub>("/hub/taskboardhub").RequireCors("CorsPolicy");
});

//app.MapControllers();

app.Run();
