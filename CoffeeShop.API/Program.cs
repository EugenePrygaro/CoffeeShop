using dotenv.net;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Порт фронтенду
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


// --- ЗАВАНТАЖЕННЯ НАЛАШТУВАНЬ ---
DotEnv.Load();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

// --- РЕЄСТРАЦІЯ СЕРВІСІВ (Dependency Injection) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- НАЛАШТУВАННЯ HTTP-КОНВЕЄРА (Middleware) ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.MapControllers();

// --- ПЕРЕВІРКА ПІДКЛЮЧЕННЯ ТА ІНІЦІАЛІЗАЦІЯ БАЗИ ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        Console.WriteLine("Спроба підключення до бази даних MySQL...");

        if (!context.Database.CanConnect())
        {
            Console.WriteLine("КРИТИЧНА ПОМИЛКА: Неможливо підключитися до бази даних.");
            Console.WriteLine("Переконайтеся, що сервер MySQL запущений, а рядок підключення у файлі .env вказано правильно.");
            Environment.Exit(1);
        }

        Console.WriteLine("Підключення до бази даних успішно встановлено.");
        context.Database.EnsureCreated();
        // Викликаємо метод очищення та наповнення тільки якщо підключення є
        DbInitializer.Initialize(context);
        Console.WriteLine("Базу даних успішно ініціалізовано тестовими даними.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("КРИТИЧНА ПОМИЛКА: Виникла проблема під час конфігурації підключення.");
        Console.WriteLine($"Деталі помилки: {ex.Message}");
        Environment.Exit(1);
    }
}

app.Run();