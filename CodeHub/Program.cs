using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CodeHub.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(120);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});


builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = $"/Admin/Authentication/Login";
	options.LogoutPath = $"/Admin/Authentication/Logout";
	options.AccessDeniedPath = $"/403";
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        string originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/404";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();


app.UseAuthorization();


// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
		var userManager = services.GetRequiredService<UserManager<User>>();
		await RolesInitializer.SeedRolesAsync(roleManager);
		await RolesInitializer.SeedAdminAsync(userManager);
	}
	catch (Exception ex)
	{
		// Log or handle the exception as needed
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred seeding the DB.");
	}
}

/**/
app.MapControllerRoute(
	name: "default",
	//pattern: "{area=Admin}/{controller=Dashboard}/{action=Index}/{id?}");
	pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
