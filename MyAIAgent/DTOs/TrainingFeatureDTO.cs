using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAIAgent.DTOs
{
    public class TrainingFeatureDTO
    {
        // turning data to numbers for complex math
        public float InstrumentId { get; set; }
        public float GenreId { get; set; }
        public float AvgBPM { get; set; }
        public float TrackLengthSeconds { get; set; }
        //public bool HasGroup { get; set; } 

        // The Label. the things we are predicting 
        public bool IsProfessional { get; set; }
    }
}
