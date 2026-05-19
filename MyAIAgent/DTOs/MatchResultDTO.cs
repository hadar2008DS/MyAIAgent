
using System.Collections.Generic;

namespace MyAIAgent.DTOs
{
    public class MatchResultDTO
    {
        public int MusicianId { get; set; }
        public int ProducerId { get; set; }
        public string MusicianName { get; set; } = string.Empty;
        public string ProducerName { get; set; } = string.Empty;
        public double Score { get; set; }
        public List<string> MatchedCriteria { get; set; } = new();
        public int BPMDifference { get; set; }
        public List<string> CommonApps { get; set; } = new();
    }
}