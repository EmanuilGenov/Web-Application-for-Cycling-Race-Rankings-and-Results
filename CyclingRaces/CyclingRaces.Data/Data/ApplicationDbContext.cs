using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CyclingRaces.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Cyclist> Cyclists { get; set; }
		public DbSet<Result> Results { get; set; }
		public DbSet<Race> Races { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Organiser> Organisers { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Cyclist>()
				.HasOne(c => c.Team)
				.WithMany(t => t.Cyclists)
				.HasForeignKey(c => c.TeamId);

			modelBuilder.Entity<Result>()
				.HasOne(r => r.Cyclist)
				.WithMany(c => c.Results)
				.HasForeignKey(r => r.CyclistId);

			modelBuilder.Entity<Race>()
				.HasOne(r => r.Organiser)
				.WithMany(o => o.Races)
				.HasForeignKey(r => r.OrganiserId);

		}
	}
}
