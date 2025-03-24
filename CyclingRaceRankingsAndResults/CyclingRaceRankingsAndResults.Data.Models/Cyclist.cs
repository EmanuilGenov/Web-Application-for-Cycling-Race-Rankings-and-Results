using Microsoft.AspNetCore.Identity;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Cyclist 
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public DateTime DateOfBirth { get; set; }
		public string Nationality { get; set; } = null!;
		public string? TeamId { get; set; }
		public Team? Team { get; set; }
		public ICollection<Result>? Results { get; set; } 
		public ICollection<Participation>? Participations { get; set; }
	}
}
