using dotenv.net;                    // Бібліотека для зчитування ключів з файлу .env
using Microsoft.EntityFrameworkCore; // Ядро Entity Framework для роботи з базою
using CoffeeShop.Infrastructure;    // Підключення твого проєкту інфраструктури (де AppDbContext)

var builder = WebApplication.CreateBuilder(args);

// --- ЗАВАНТАЖЕННЯ НАЛАШТУВАНЬ ---
// Завантажуємо змінні оточення з файлу .env 
DotEnv.Load();

// Витягуємо рядок підключення до MySQL
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

// --- РЕЄСТРАЦІЯ СЕРВІСІВ (Dependency Injection) ---

// Реєструємо базу даних MySQL у системі
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString, 
        ServerVersion.AutoDetect(connectionString)
    ));

// Додаємо підтримку контролерів 
builder.Services.AddControllers();

// Налаштування документації API (OpenAPI/Swagger)
builder.Services.AddOpenApi();

var app = builder.Build();

// --- НАЛАШТУВАННЯ HTTP-КОНВЕЄРА (Middleware) ---

// Якщо ми в режимі розробки — вмикаємо візуальну документацію запитів
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Автоматичне перенаправлення на безпечне з'єднання (HTTPS)
app.UseHttpsRedirection();

// --- МАРШРУТИЗАЦІЯ ---

// Тут система автоматично підтягує всі методи з папки Controllers
app.MapControllers();

// --- ТИМЧАСОВИЙ БЛОК ДЛЯ ОНОВЛЕННЯ БАЗИ ДАНИХ ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    // Викликаємо твій метод очищення та наповнення
    DbInitializer.Initialize(context);
}
// ------------------------------------------------

// Тут пізніше можна буде додати специфічні налаштування для логування або авторизації
app.Run(); // Запуск всього бекенду


