using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Stage
	{
		public int Id { get; set; }
		public int RaceId { get; set; }
		public Race Race { get; set; } = null!;
		public int StageNumber { get; set; }
		public decimal Distance { get; set; }
		public string StartLocation { get; set; } = null!;
		public string EndLocation { get; set; } = null!;
		public ICollection<Result>? Results { get; set; }
	}
}
