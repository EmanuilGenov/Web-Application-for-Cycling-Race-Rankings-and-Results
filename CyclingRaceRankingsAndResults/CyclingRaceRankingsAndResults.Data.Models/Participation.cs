using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Participation
	{
		public int Id { get; set; }
		public int RaceId { get; set; }
		public Race Race { get; set; } = null!;
		public int CyclistId { get; set; }
		public Cyclist Cyclist { get; set; } = null!;
		public TimeSpan? OverallTime { get; set; }
		public int OverallRank { get; set; }
	}
}
