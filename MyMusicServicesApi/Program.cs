using MyMusicServicesApi.Services;
using Microsoft.EntityFrameworkCore;
using MyMusicServicesApi.Data;

var builder = WebApplication.CreateBuilder(args);

//add services to the container.
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddSingleton(builder.Configuration.GetSection("Jwt").GetValue<string>("Key"));

builder.Services.AddSingleton<PasswordHasher>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=users.db"));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();
app.Run();