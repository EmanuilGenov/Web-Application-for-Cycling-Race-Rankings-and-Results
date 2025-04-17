using System.ComponentModel.DataAnnotations;

namespace CyclingRaces.Data.Models
{
	public class Organiser
	{
		[Key]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public ICollection<Race>? Races { get; set; } = new List<Race>();
	}
}