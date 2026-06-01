using FUNewsManagementSystem.Data;
using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<NewsManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<NewsArticleRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<NewsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NewsManagementDbContext>();
    context.Database.EnsureCreated();
    SeedDemoAccounts(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapHub<FUNewsManagementSystem.Web.Hubs.NewsHub>("/newsHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void SeedDemoAccounts(NewsManagementDbContext context)
{
    UpsertAccount(context, new SystemAccount
    {
        AccountId = 1,
        AccountName = "Admin Demo",
        AccountEmail = "admin@FUNewsManagementSystem.org",
        AccountRole = AccountRole.Admin,
        AccountPassword = "@@abc123@@"
    });

    UpsertAccount(context, new SystemAccount
    {
        AccountId = 2,
        AccountName = "Staff Demo",
        AccountEmail = "staff@funews.org",
        AccountRole = AccountRole.Staff,
        AccountPassword = "123"
    });

    UpsertAccount(context, new SystemAccount
    {
        AccountId = 3,
        AccountName = "Lecturer Demo",
        AccountEmail = "lecturer@funews.org",
        AccountRole = AccountRole.Lecturer,
        AccountPassword = "123"
    });

    context.SaveChanges();
}

static void UpsertAccount(NewsManagementDbContext context, SystemAccount account)
{
    var existing = context.SystemAccounts.SingleOrDefault(a => a.AccountId == account.AccountId);
    if (existing is null)
    {
        context.SystemAccounts.Add(account);
        return;
    }

    existing.AccountName = account.AccountName;
    existing.AccountEmail = account.AccountEmail;
    existing.AccountRole = account.AccountRole;
    existing.AccountPassword = account.AccountPassword;
}
