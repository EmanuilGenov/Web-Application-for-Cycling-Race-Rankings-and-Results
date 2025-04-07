using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaces.Data.Models
{
	public class Cyclist : IdentityUser
	{
		public string Nationality { get; set; } = null!;
		public DateTime DateOfBirth { get; set; }
		public string? TeamId { get; set; }
		public Team? Team { get; set; }
		public ICollection<Result>? Results { get; set; }
		public ICollection<Participation>? Participations { get; set; }
	}
}
