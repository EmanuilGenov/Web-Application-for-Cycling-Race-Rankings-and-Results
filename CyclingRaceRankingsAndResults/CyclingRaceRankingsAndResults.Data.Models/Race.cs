using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Race
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Location { get; set; } = null!;
		public DateTime Date { get; set; }
		public string Type { get; set; } = null!;
		public double Distance { get; set; }

		public ICollection<Stage> Stages { get; set; } = null!;
		public ICollection<Participation> Participations { get; set; } = null!;
		public string OrganiserName { get; set; } = null!;
		public Organiser Organiser { get; set; } = null!;
	}
}
