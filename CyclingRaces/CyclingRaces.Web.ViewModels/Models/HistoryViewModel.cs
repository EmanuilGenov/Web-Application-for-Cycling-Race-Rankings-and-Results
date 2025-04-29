using CyclingRaces.Data.Models;

namespace CyclingRaces.Data.ViewModels
{
    public class HistoryViewModel
    {
        public ICollection<Result> StageResults { get; set; } = new List<Result>();
        public ICollection<Race> CreatedRaces { get; set; } = new List<Race>();
    }
}