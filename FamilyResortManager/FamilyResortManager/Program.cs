using FamilyResortManager.Components;
using FamilyResortManager.Data;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Services;
using FamilyResortManager.Services.Authentication;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.Mapping;
using FamilyResortManager.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies());

// Добавление служб Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Настройка cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

// Настройка Razor Pages с политиками авторизации
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "RequireAdministratorRole");
    options.Conventions.AuthorizeFolder("/Employee", "RequireEmployeeRole");
});

// Добавление политик авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireRole("Administrator"));
    options.AddPolicy("RequireEmployeeRole", policy =>
        policy.RequireRole("Administrator", "Employee"));
});

builder.Services.AddSingleton<TelegramNotifier>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddApplicationServices();
builder.Services.AddValidatorsFromAssemblyContaining<BookingCreateRequestDtoValidator>();

var app = builder.Build();

// Настройка промежуточного ПО
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

// Перенаправление конечных слешей
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (path != "/" && path.EndsWith("/"))
    {
        context.Response.Redirect(path.TrimEnd('/') + context.Request.QueryString, permanent: true);
        return;
    }
    await next();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Identity/Account/AccessDeniedPage");

app.MapRazorPages();

// Инициализация ролей и пользователей при запуске
await IdentityDataInitializer.InitializeAsync(app.Services);

app.Run();