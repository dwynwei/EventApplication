using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("EtkinlikYonetimDb")));

builder.Services.AddLogging(builder => builder.AddFilter("Microsoft", LogLevel.Warning));

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    }).AddCookie(opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.LogoutPath = "/Account/Logout";
    });

builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    /*options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;*/
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<EventDBContext>()
.AddDefaultTokenProviders();

var serviceProvider = builder.Services.BuildServiceProvider();
var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var adminUser = await userManager.FindByNameAsync("admin@example.com");
if (adminUser == null)
{
    adminUser = new ApplicationUser
    {
        FirstName = "Cagatay",
        LastName = "Sahin",
        UserName = "admin@example.com",
        Email = "admin@example.com",
        Salt = "1"
    };
    IdentityResult result = await userManager.CreateAsync(adminUser, "123456");
    if (result.Succeeded)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddMvc();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var queryString = context.Request.QueryString.Value;

    if (!string.IsNullOrEmpty(queryString) && queryString.Length > 2048)
    {
        context.Response.StatusCode = 414;
        await context.Response.WriteAsync("The query string is too long.");
    }
    else
    {
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
