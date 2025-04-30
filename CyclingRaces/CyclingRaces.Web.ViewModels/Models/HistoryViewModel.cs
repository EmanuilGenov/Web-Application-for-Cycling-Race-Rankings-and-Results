using CyclingRaces.Data.Models;

namespace CyclingRaces.Data.ViewModels
{
    public class HistoryViewModel
    {
        public List<Result> StageResults { get; set; } = new List<Result>();
        public List<Race> CreatedRaces { get; set; } = new List<Race>();
    }
}