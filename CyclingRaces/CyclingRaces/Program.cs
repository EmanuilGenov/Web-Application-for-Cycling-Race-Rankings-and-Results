using CyclingRaces.Data;
using CyclingRaces.Data.Data;
using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyclingRaces
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
				options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

			var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                DatabaseSeeder.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			
			app.Run();
		}
	}
}
