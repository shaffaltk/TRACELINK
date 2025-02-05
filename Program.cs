using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using TraceLink.Data;
using TraceLink.Models;
using TraceLink.Hubs;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Razor Pages with global authorization
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Protect all Razor Pages
});

// ✅ Configure Database Context with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configure Identity with custom user and role management
builder.Services.AddIdentity<TraceLinkUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// ✅ Register HttpClient with a base address for API calls
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001"); // Change to match your API URL
});

// ✅ Register SignalR for real-time chat
builder.Services.AddSignalR();

// ✅ Build the application
var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Map Razor Pages and SignalR Hubs
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub"); // Ensure the hub is mapped correctly

// ✅ Seed roles and admin user on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdminAsync(services);
}

// ✅ Run the application
app.Run();

/// <summary>
/// Seeds roles and an admin user on application startup.
/// </summary>
static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<TraceLinkUser>>();

    var roles = new[] { "Admin", "Dealer", "Subdealer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
            {
                Console.WriteLine($"Error creating role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }

    // ✅ Seed an Admin User
    var adminEmail = "admin@tracelink.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new TraceLinkUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var adminResult = await userManager.CreateAsync(newAdmin, "Admin@123"); // Ensure a strong password
        if (adminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
            Console.WriteLine($"Admin user '{adminEmail}' created successfully.");
        }
        else
        {
            Console.WriteLine($"Error creating admin user: {string.Join(", ", adminResult.Errors.Select(e => e.Description))}");
        }
    }
}
