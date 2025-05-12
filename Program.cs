using Microsoft.EntityFrameworkCore;
using FormForPF.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка сервисов
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Middleware - порядок важен! =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Включаем поддержку HTTPS
app.UseStaticFiles();       // Обслуживаем статические файлы из wwwroot
app.UseCors("AllowAll");   // ✅ Только один раз, до MapControllers
app.UseAuthorization();    // Авторизация (если нужна)

app.MapControllers();      // Маршруты контроллеров

app.Run();