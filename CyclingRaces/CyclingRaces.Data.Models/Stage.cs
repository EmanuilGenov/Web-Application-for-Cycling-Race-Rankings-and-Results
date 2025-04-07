using System.Diagnostics;

namespace CyclingRaces.Data.Models
{
	public class Stage
	{
		public string Id { get; set; } = null!;
		public string RaceId { get; set; } = null!;
		public Race Race { get; set; } = null!;
		public int StageNumber { get; set; }
		public double Distance { get; set; }
		public string StartLocation { get; set; } = null!;
		public string EndLocation { get; set; } = null!;
		public ICollection<Result>? Results { get; set; }
	}
}