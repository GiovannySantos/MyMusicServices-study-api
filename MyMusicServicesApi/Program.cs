using MyMusicServicesApi.Services;
using Microsoft.EntityFrameworkCore;
using MyMusicServicesApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5177");

//planejar uma estratégia melhor para armazenar a chave secreta, como usar variáveis de ambiente ou um serviço de gerenciamento de segredos
builder.Services.AddScoped<AuthService>();

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