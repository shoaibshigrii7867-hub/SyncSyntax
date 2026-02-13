using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using SyncSyntax.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 1;
    
}).AddEntityFrameworkStores<ApDbContext>();
builder.Services.ConfigureApplicationCookie(Option =>
{
    Option.LoginPath = "/Auth/Login";
    Option.AccessDeniedPath = "/Post/AccessDenied";
    Option.ExpireTimeSpan = TimeSpan.FromHours(4);
    Option.SlidingExpiration = true;
}
);
var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
     var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
     var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "admin@gmail.com";
    string adminPassword = "admin";
    var ExistingAdminRole = await _roleManager.FindByNameAsync("Admin");
    if(ExistingAdminRole == null)
    {
         await _roleManager.CreateAsync(new IdentityRole("Admin"));

    }
    var ExistingAdminUser = await _userManager.FindByEmailAsync(adminEmail);
    if (ExistingAdminUser == null)
    {
        var adminUser= new IdentityUser { UserName=adminEmail, Email=adminEmail};
        await _userManager.CreateAsync(adminUser, adminPassword);
        await _userManager.AddToRoleAsync(adminUser, "Admin");        
    }


}






// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
