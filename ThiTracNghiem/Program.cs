using ThiTracNghiem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
}

app.Run();

// async Task CreateRoles(IServiceProvider serviceProvider)
// {
//     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

//     string[] roleNames = { "Admin", "User" };
//     IdentityResult roleResult;

//     foreach (var roleName in roleNames)
//     {
//         var roleExist = await roleManager.RoleExistsAsync(roleName);
//         if (!roleExist)
//         {
//             roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
//         }
//     }

//     // Tạo tài khoản Admin mặc định
//     var adminUser = new IdentityUser
//     {
//         UserName = "admin@gmail.com",
//         Email = "admin@gmail.com",
//         EmailConfirmed = true
//     };

//     string adminPassword = "Admin@123";

//     var user = await userManager.FindByEmailAsync(adminUser.Email);

//     if (user == null)
//     {
//         var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
//         if (createPowerUser.Succeeded)
//         {
//             await userManager.AddToRoleAsync(adminUser, "Admin");
//         }
//     }
// }


