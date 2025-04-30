using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CyclingRaces.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
                .HasForeignKey(c => c.TeamId)
                .OnDelete(DeleteBehavior.Restrict); // Optional, but may help prevent issues

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Cyclist)
                .WithMany(c => c.Results)
                .HasForeignKey(r => r.CyclistId)
                .OnDelete(DeleteBehavior.Cascade); // Probably okay to cascade here

            modelBuilder.Entity<Race>()
                .HasOne(r => r.Organiser)
                .WithMany(o => o.Races)
                .HasForeignKey(r => r.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict); // 🔹 IMPORTANT: prevents cascade loop

            modelBuilder.Entity<Cyclist>()
                .HasOne(c => c.User)
                .WithOne(u => u.CyclistProfile)
                .HasForeignKey<Cyclist>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // 🔹 IMPORTANT

            modelBuilder.Entity<Organiser>()
                .HasOne(o => o.User)
                .WithOne(u => u.OrganiserProfile)
                .HasForeignKey<Organiser>(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
	}
}
