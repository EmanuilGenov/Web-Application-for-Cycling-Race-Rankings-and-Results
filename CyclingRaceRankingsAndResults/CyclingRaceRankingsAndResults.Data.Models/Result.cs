using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaceRankingsAndResults.Data.Models
{
	public class Result
	{
		public int Id { get; set; }
		public int StageId { get; set; }
		public Stage Stage { get; set; } = null!;
		public int CyclistId { get; set; }
		public Cyclist Cyclist { get; set; } = null!;
		public TimeSpan? Time { get; set; }
		public int Rank { get; set; }
	}
}
