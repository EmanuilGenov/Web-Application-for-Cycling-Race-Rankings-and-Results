using System.ComponentModel.DataAnnotations;

namespace CyclingRaces.Data.Models
{
	public class Organiser : ApplicationUser
	{
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Race>? Races { get; set; } = new List<Race>();
	}
}