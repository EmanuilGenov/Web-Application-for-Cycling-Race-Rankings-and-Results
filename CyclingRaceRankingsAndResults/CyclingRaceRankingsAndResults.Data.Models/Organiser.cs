using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Organiser
	{
		[Key]
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public ICollection<Race>? Races { get; set; } = new List<Race>();
	}
}
