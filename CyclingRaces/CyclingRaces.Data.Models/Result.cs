namespace CyclingRaces.Data.Models
{
	public class Result
	{
		public string Id { get; set; } = null!;
		public string StageId { get; set; } = null!;
		public Stage Stage { get; set; } = null!;
		public string CyclistId { get; set; } = null!;
		public Cyclist Cyclist { get; set; } = null!;
		public TimeSpan? Time { get; set; }
		public int Rank { get; set; }
	}
}