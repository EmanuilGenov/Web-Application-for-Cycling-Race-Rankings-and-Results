using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public class SeedRoles
{
	public static async Task Initialize(IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		string[] roleNames = { "Admin", "User", "Manager" };

		foreach (var role in roleNames)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}
		}
	}
}
