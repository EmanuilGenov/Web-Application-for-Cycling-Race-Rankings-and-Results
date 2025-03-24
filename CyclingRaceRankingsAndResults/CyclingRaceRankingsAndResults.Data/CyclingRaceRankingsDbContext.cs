using CyclingRaceRankingsAndResults.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace CyclingRaceRankingsAndResults.Data
{
	public class CyclingRaceRankingsDbContext : IdentityDbContext<IdentityUser>
	{
		public CyclingRaceRankingsDbContext(DbContextOptions<CyclingRaceRankingsDbContext> options)
            : base(options)
		{
		}


		public DbSet<Cyclist> Cyclists { get; set; }
		public DbSet<Result> Results { get; set; }
		public DbSet<Stage> Stages { get; set; }
		public DbSet<Race> Races { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Participation> Participations { get; set; }
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

			modelBuilder.Entity<Result>()
				.HasOne(r => r.Stage)
				.WithMany(s => s.Results)
				.HasForeignKey(r => r.StageId);

			modelBuilder.Entity<Stage>()
				.HasOne(s => s.Race)
				.WithMany(r => r.Stages)
				.HasForeignKey(s => s.RaceId);

			modelBuilder.Entity<Participation>()
				.HasOne(p => p.Race)
				.WithMany(r => r.Participations)
				.HasForeignKey(p => p.RaceId);

			modelBuilder.Entity<Participation>()
				.HasOne(p => p.Cyclist)
				.WithMany(c => c.Participations)
				.HasForeignKey(p => p.CyclistId);

			modelBuilder.Entity<Race>()
				.HasOne(r => r.Organiser)
				.WithMany(o => o.Races)
				.HasForeignKey(r => r.OrganiserName);

		}
	}
}